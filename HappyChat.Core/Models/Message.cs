using System;

namespace HappyChat.Core.Models;

public class Message
{
    public int Id { get; set; }

    public int ChatId { get; set; }

    public int SenderId { get; set; }

    public int? RepliedTo { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime SentAt { get; set; }

    public MessageStatus Status { get; set; }

    public string? Reaction { get; set; }
}