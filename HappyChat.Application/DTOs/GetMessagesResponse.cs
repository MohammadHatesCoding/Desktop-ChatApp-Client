using HappyChat.Shared.Enum;

namespace HappyChat.Application.DTOs.Chat;

public sealed record GetMessagesResponse(
    int Id,
    int SenderId,
    string SenderName,
    string Content,
    int? RepliedTo,
    MessageStatus Status,
    DateTime SentAt,
    bool IsEdited,
    bool IsMine);