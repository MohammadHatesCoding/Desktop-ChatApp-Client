using HappyChat.Application.DTOs;
using HappyChat.Core.Models;

namespace HappyChat.Desktop.ViewModels.Chat;

public sealed class MessageViewModel : ViewModelBase
{
    public MessageViewModel(ChatMessageDto dto, string senderInitials)
    {
        Content = dto.Content;
        IsMine = dto.IsMine;
        IsRead = dto.Status == MessageStatus.Read;
        Reaction = dto.Reaction;
        SenderInitials = senderInitials;
        TimeText = dto.SentAt.ToString("h:mm tt");
    }

    public string Content { get; }

    public bool IsMine { get; }

    public bool IsRead { get; }

    public string? Reaction { get; }

    public string SenderInitials { get; }

    public string TimeText { get; }
}