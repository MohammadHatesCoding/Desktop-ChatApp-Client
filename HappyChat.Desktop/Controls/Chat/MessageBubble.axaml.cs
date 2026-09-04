using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using HappyChat.Desktop.Services;
using HappyChat.Desktop.ViewModels.Chat;
using HappyChat.Desktop.Views.Chat;

namespace HappyChat.Desktop.Controls.Chat;

public partial class MessageBubble : UserControl
{
    private MessageContextMenuService? _contextMenuService;

    public MessageBubble()
    {
        InitializeComponent();

        PointerPressed += OnPointerPressed;
    }

    private void OnPointerPressed(
        object? sender,
        PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            return;

        if (DataContext is not MessageItemViewModel message)
            return;

        var chatViewModel =
            this.FindAncestorOfType<ChatView>()?
                .DataContext as ChatViewModel;

        if (chatViewModel is null)
            return;

        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel is null)
            return;

        _contextMenuService =
            new MessageContextMenuService(chatViewModel);

        var menu =
            _contextMenuService.Create(
                message,
                topLevel);

        menu.ShowAt(this, true);

        e.Handled = true;
    }

    private void ReplyPreview_OnPointerPressed(
        object? sender,
        PointerPressedEventArgs e)
    {
        if (DataContext is not MessageItemViewModel message)
            return;

        if (message.ReplyTargetId is null)
            return;

        var chatViewModel =
            this.FindAncestorOfType<ChatView>()?
                .DataContext as ChatViewModel;

        chatViewModel?.ScrollToMessage(
            message.ReplyTargetId.Value);
    }
}