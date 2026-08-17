using HappyChat.Core.Models;

namespace HappyChat.Application.DTOs;

public sealed record ChatMessageDto(int Id, int SenderId, int? RepliedTo, string Content, 
    DateTime SentAt, bool IsMine, MessageStatus Status, string? Reaction);