namespace HappyChat.Core.Models;

public class Chat
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public bool IsOnline { get; set; }

    public DateTime? LastSeenAt { get; set; }
}