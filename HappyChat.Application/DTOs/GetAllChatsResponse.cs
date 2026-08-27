namespace HappyChat.Application.DTOs.Chat;

public sealed record GetAllChatsResponse(
    int ChatId,
    string Title,
    string? LastMessage,
    DateTime? LastMessageTime,
    bool IsOnline,
    int UnreadCount);