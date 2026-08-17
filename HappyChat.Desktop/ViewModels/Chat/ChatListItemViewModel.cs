using System;
using System.Windows.Input;
using Avalonia.Media;
using HappyChat.Application.DTOs;
using HappyChat.Desktop.Helpers;

namespace HappyChat.Desktop.ViewModels.Chat;

public sealed class ChatListItemViewModel : ViewModelBase
{
    private bool _isSelected;

    public ChatListItemViewModel(ChatSummaryDto dto, ICommand selectCommand)
    {
        Id = dto.Id;
        Name = dto.Name;
        Initials = dto.Initials;
        LastMessage = dto.LastMessage;
        UnreadCount = dto.UnreadCount;
        IsOnline = dto.IsOnline;
        TimeText = FormatTime(dto.LastMessageAt);
        AvatarBrush = AvatarColorPalette.GetColor(dto.Id);
        SelectCommand = selectCommand;
    }

    public int Id { get; }

    public string Name { get; }

    public string Initials { get; }

    public string LastMessage { get; }

    public int UnreadCount { get; }

    public bool IsOnline { get; }

    public string TimeText { get; }

    public IBrush AvatarBrush { get; }

    public ICommand SelectCommand { get; }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    private static string FormatTime(DateTime dateTime)
    {
        var delta = DateTime.Now - dateTime;

        if (delta.TotalMinutes < 1)
            return "now";

        if (delta.TotalMinutes < 60)
            return $"{(int)delta.TotalMinutes}m";

        if (delta.TotalHours < 24)
            return $"{(int)delta.TotalHours}h";

        return $"{(int)delta.TotalDays}d";
    }
}