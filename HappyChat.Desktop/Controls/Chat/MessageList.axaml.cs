using Avalonia.Controls;
using Avalonia.Threading;
using HappyChat.Desktop.ViewModels.Chat;
using System.Collections.Specialized;

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
}