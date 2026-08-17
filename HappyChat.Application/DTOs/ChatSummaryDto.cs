using System;

namespace HappyChat.Application.DTOs;

public sealed record ChatSummaryDto(
    int Id,
    string Name,
    string Initials,
    string LastMessage,
    DateTime LastMessageAt,
    int UnreadCount,
    bool IsOnline);