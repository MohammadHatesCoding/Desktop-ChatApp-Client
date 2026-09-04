using Avalonia.Controls;
using Avalonia.Input.Platform;
using HappyChat.Desktop.Commands;
using HappyChat.Desktop.Controls.Chat;
using HappyChat.Desktop.ViewModels.Chat;
using HappyChat.Shared.Enum;
using System;

namespace HappyChat.Desktop.Services;

public sealed class MessageContextMenuService
{
    private readonly ChatViewModel _chatViewModel;

    public MessageContextMenuService(ChatViewModel chatViewModel)
    {
        _chatViewModel = chatViewModel;
    }

    public MenuFlyout Create(
        MessageItemViewModel message,
        TopLevel topLevel)
    {
        var menu = new MenuFlyout();

        menu.Items.Add(
            CreateItem(
                "Reply",
                () => _chatViewModel.ReplyToMessage(message)));

        if (message.IsMine)
        {
            if (message.Type == MessageType.Text)
            {
                menu.Items.Add(
                    CreateItem(
                        "Edit",
                        () => _chatViewModel.EditMessage(message)));

                menu.Items.Add(
                    CreateItem(
                        "Copy Text",
                        () => CopyTextAsync(message, topLevel)));

                menu.Items.Add(
                    CreateItem(
                        "Delete",
                        () => DeleteMessageAsync(message, topLevel)));
            }
            else
            {
                menu.Items.Add(
                    CreateItem(
                        "Edit",
                        () => { }));

                menu.Items.Add(
                    CreateItem(
                        "Delete",
                        () => { }));

                menu.Items.Add(
                    CreateItem(
                        "Download",
                        () => { }));
            }
        }
        else
        {
            if (message.Type == MessageType.Text)
            {
                menu.Items.Add(
                    CreateItem(
                        "Copy Text",
                        () => CopyTextAsync(message, topLevel)));
            }
            else
            {
                menu.Items.Add(
                    CreateItem(
                        "Download",
                        () => { }));
            }
        }

        return menu;
    }

    private async void DeleteMessageAsync(
        MessageItemViewModel message,
        TopLevel topLevel)
    {
        if (topLevel is not Window owner)
            return;

        var chatTitle =
            _chatViewModel.SelectedConversationName;

        var dialog =
            new DeleteMessageDialog(chatTitle);

        var deleteType =
            await dialog.ShowDialog<DeleteType?>(owner);

        if (deleteType is null)
            return;

        try
        {
            await _chatViewModel.DeleteMessageAsync(
                message,
                deleteType.Value);
        }
        catch
        {
            // Error handling can be added later.
        }
    }

    private static async void CopyTextAsync(
        MessageItemViewModel message,
        TopLevel topLevel)
    {
        if (topLevel.Clipboard is null)
            return;

        await topLevel.Clipboard.SetTextAsync(message.Text);
    }

    private static MenuItem CreateItem(
        string header,
        Action action)
    {
        return new MenuItem
        {
            Header = header,
            Command = new RelayCommand(action)
        };
    }
}