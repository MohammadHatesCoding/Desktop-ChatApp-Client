using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HappyChat.Desktop.ViewModels.Chat;
using System.Collections.Specialized;
using System.Linq;

namespace HappyChat.Desktop.Controls.Chat;


public partial class MessageList : UserControl
{
    private ChatViewModel? _chatViewModel;
    public MessageList()
    {
        InitializeComponent();

        DataContextChanged += OnDataContextChanged;

        SizeChanged += OnSizeChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        if (_chatViewModel is not null)
        {
            _chatViewModel.Messages.CollectionChanged -= OnMessagesCollectionChanged;
        }

        _chatViewModel = DataContext as ChatViewModel;

        if (_chatViewModel is not null)
        {
            _chatViewModel.Messages.CollectionChanged += OnMessagesCollectionChanged;

            _chatViewModel.ScrollToMessageRequested += ScrollToMessage;

            ScrollToBottom();
        }
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        ScrollToBottom();
    }

    private void OnMessagesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ScrollToBottom();
    }

    private void ScrollToBottom()
    {
        Dispatcher.UIThread
            .Post(() => { MessageScrollViewer.ScrollToEnd(); },
                DispatcherPriority.Loaded);
    }

    private void ScrollToMessage(int messageId)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var index = FindMessageIndex(messageId);

            if (index < 0)
                return;


            var container =
                MessagesList.GetVisualDescendants()
                    .OfType<Control>()
                    .FirstOrDefault(x =>
                        x.DataContext is MessageItemViewModel message &&
                        message.Id == messageId);


            container?.BringIntoView();

        });
    }

    private int FindMessageIndex(int messageId)
    {
        if (_chatViewModel is null)
            return -1;

        for (int i = 0; i < _chatViewModel.Messages.Count; i++)
        {
            if (_chatViewModel.Messages[i].Id == messageId)
                return i;
        }

        return -1;
    }
}