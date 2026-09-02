using Avalonia.Controls;
using HappyChat.Desktop.Commands;
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
    public MenuFlyout Create(MessageItemViewModel message)
    {
        var menu = new MenuFlyout();


        menu.Items.Add(
            CreateItem(
                "Reply",
                () =>
                {
                    _chatViewModel.ReplyToMessage(message);
                }));


        if (message.IsMine)
        {
            if (message.Type == MessageType.Text)
            {
                menu.Items.Add(
                    CreateItem(
                        "Edit",
                        () =>
                        {
                            // بعداً Edit
                        }));


                menu.Items.Add(
                    CreateItem(
                        "Copy Text",
                        () =>
                        {
                            // بعداً Clipboard
                        }));


                menu.Items.Add(
                    CreateItem(
                        "Delete",
                        () =>
                        {
                            // بعداً Delete
                        }));
            }
            else
            {
                menu.Items.Add(
                    CreateItem(
                        "Edit",
                        () =>
                        {
                        }));

                menu.Items.Add(
                    CreateItem(
                        "Delete",
                        () =>
                        {
                        }));

                menu.Items.Add(
                    CreateItem(
                        "Download",
                        () =>
                        {
                        }));
            }
        }
        else
        {
            if (message.Type == MessageType.Text)
            {
                menu.Items.Add(
                    CreateItem(
                        "Copy Text",
                        () =>
                        {
                        }));
            }
            else
            {
                menu.Items.Add(
                    CreateItem(
                        "Download",
                        () =>
                        {
                        }));
            }
        }


        return menu;
    }


    private MenuItem CreateItem(
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