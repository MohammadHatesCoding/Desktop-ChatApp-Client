using HappyChat.Application.DTOs.Chat;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyChat.Desktop.ViewModels.Chat;

public sealed class ConversationItemViewModel :
    INotifyPropertyChanged
{
    private static readonly string[] AvatarBrushes =
    [
        "#2563EB",
        "#8B5CF6",
        "#10B981",
        "#EC4899",
        "#F97316",
        "#06B6D4",
        "#6366F1",
        "#3B82F6"
    ];

    private bool _isSelected;

    public ConversationItemViewModel(
        GetAllChatsResponse chat,
        int index)
    {
        ChatId = chat.ChatId;
        Title = chat.Title;
        LastMessage = chat.LastMessage ?? string.Empty;
        LastMessageTime = chat.LastMessageTime;
        IsOnline = chat.IsOnline;
        UnreadCount = chat.UnreadCount;

        AvatarBrush =
            AvatarBrushes[
                index % AvatarBrushes.Length];
    }

    public event PropertyChangedEventHandler?
        PropertyChanged;


    public int ChatId { get; }

    public string Title { get; }

    public string LastMessage { get; }

    public DateTime? LastMessageTime { get; }

    public bool IsOnline { get; }

    public int UnreadCount { get; }

    public string AvatarBrush { get; }


    public bool IsSelected
    {
        get => _isSelected;

        set
        {
            if (_isSelected == value)
                return;

            _isSelected = value;

            OnPropertyChanged();
        }
    }


    public bool HasUnread =>
        UnreadCount > 0;


    public string Initials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Title))
                return "?";

            var parts =
                Title
                    .Split(
                        ' ',
                        StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
                return parts[0][..1]
                    .ToUpperInvariant();

            return
                $"{parts[0][0]}{parts[^1][0]}"
                    .ToUpperInvariant();
        }
    }


    public string TimeText
    {
        get
        {
            if (LastMessageTime is null)
                return string.Empty;

            var time =
                LastMessageTime.Value;

            if (time.Date == DateTime.Today)
                return time.ToString("h:mm tt");

            if (time.Date ==
                DateTime.Today.AddDays(-1))
            {
                return "Yesterday";
            }

            return time.ToString("MMM d");
        }
    }


    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?
            .Invoke(
                this,
                new PropertyChangedEventArgs(
                    propertyName));
    }
}