using HappyChat.Shared.Enum;

namespace HappyChat.Application.DTOs.Chat;

public sealed record OpenChatResponse(
    int ChatId,
    string Title,
    ChatType Type,
    ChatPrivacy Privacy,
    bool IsOnline,
    DateTime? LastSeen);