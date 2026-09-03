using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Avalonia.VisualTree;
using HappyChat.Desktop.ViewModels.Chat;
using HappyChat.Desktop.Views.Chat;
using System;
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

        MessageScrollViewer.ScrollChanged += OnScrollChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (_chatViewModel is not null)
        {
            _chatViewModel.Messages.CollectionChanged -= OnMessagesCollectionChanged;
            _chatViewModel.ScrollToMessageRequested -= ScrollToMessage;
        }

        _chatViewModel = DataContext as ChatViewModel;

        if (_chatViewModel is not null)
        {
            _chatViewModel.Messages.CollectionChanged += OnMessagesCollectionChanged;
            _chatViewModel.ScrollToMessageRequested += ScrollToMessage;

            Dispatcher.UIThread.Post(
                ScrollToBottom,
                DispatcherPriority.Loaded);
        }
    }

    private void OnSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        ScrollToBottom();
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_chatViewModel is null)
            return;

        if (_chatViewModel.IsLoadingMessages)
            return;

        if (_chatViewModel.IsLoadingOlderMessages)
            return;

        if (!_chatViewModel.HasMoreMessages)
            return;

        if (MessageScrollViewer.Extent.Height <=
            MessageScrollViewer.Viewport.Height)
        {
            return;
        }

        if (MessageScrollViewer.Offset.Y > 1)
            return;

        _ = LoadOlderMessagesAndRestorePositionAsync();
    }

    private async System.Threading.Tasks.Task LoadOlderMessagesAndRestorePositionAsync()
    {
        if (_chatViewModel is null)
            return;

        var oldOffset = MessageScrollViewer.Offset.Y;
        var oldExtentHeight = MessageScrollViewer.Extent.Height;

        await _chatViewModel.LoadOlderMessagesAsync();

        Dispatcher.UIThread.Post(
            () =>
            {
                var newExtentHeight = MessageScrollViewer.Extent.Height;

                var heightDifference =
                    newExtentHeight - oldExtentHeight;

                var newOffset = oldOffset + heightDifference;

                if (newOffset < 0)
                    newOffset = 0;

                MessageScrollViewer.Offset =
                    new Vector(
                        MessageScrollViewer.Offset.X,
                        newOffset);
            },
            DispatcherPriority.Loaded);
    }

    private void OnMessagesCollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            Dispatcher.UIThread.Post(
                ScrollToBottom,
                DispatcherPriority.Loaded);

            return;
        }

        if (e.Action != NotifyCollectionChangedAction.Add)
            return;

        // Older messages are inserted at index 0.
        // Do NOT scroll to bottom in that case.
        if (e.NewStartingIndex == 0)
            return;

        // New messages are appended at the bottom.
        Dispatcher.UIThread.Post(
            ScrollToBottom,
            DispatcherPriority.Loaded);
    }

    private void ScrollToBottom()
    {
        Dispatcher.UIThread.Post(
            () =>
            {
                MessageScrollViewer.ScrollToEnd();
            },
            DispatcherPriority.Loaded);
    }

    private void ScrollToMessage(int messageId)
    {
        Dispatcher.UIThread.Post(() =>
        {
            var container =
                MessagesList
                    .GetVisualDescendants()
                    .OfType<Control>()
                    .FirstOrDefault(x =>
                        x.DataContext is MessageItemViewModel message &&
                        message.Id == messageId);

            container?.BringIntoView();
        });
    }
}