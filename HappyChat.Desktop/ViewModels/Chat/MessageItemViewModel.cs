using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;
using HappyChat.Application.DTOs.Chat;
using HappyChat.Shared.Enum;
using System;

namespace HappyChat.Desktop.ViewModels.Chat;

public sealed class MessageItemViewModel
{
    public MessageItemViewModel(GetMessagesResponse message)
    {
        Id = message.Id;
        SenderId = message.SenderId;
        SenderName = message.SenderName;
        Text = message.Content;
        RepliedTo = message.RepliedTo;
        Status = message.Status;
        Type = MessageType.Text;
        SentAt = message.SentAt;
        IsEdited = message.IsEdited;
        IsMine = message.IsMine;

        AvatarBrush = GetAvatarBrush(SenderName);

        Initials = GetInitials(SenderName);
    }

    public int Id { get; }

    public int SenderId { get; }

    public string SenderName { get; }

    public string Text { get; }

    public int? RepliedTo { get; }

    public MessageStatus Status { get; }

    public MessageType Type { get; }

    public DateTime SentAt { get; }

    public bool IsEdited { get; }

    public bool IsMine { get; }

    public string AvatarBrush { get; }

    public string Initials { get; }

    public bool HasReply =>
        RepliedTo.HasValue;

    public bool HasReaction =>
        false;

    public string Reaction =>
        string.Empty;

    public bool ShowSentReceipt => IsMine && Status == MessageStatus.Sent;

    public bool ShowReadReceipt => IsMine && Status == MessageStatus.Seen;

    public HorizontalAlignment BubbleAlignment =>
        IsMine
            ? HorizontalAlignment.Right
            : HorizontalAlignment.Left;

    public IBrush BubbleBackground =>
        IsMine
            ? new SolidColorBrush(
                Color.Parse("#2563EB"))
            : new SolidColorBrush(
                Color.Parse("#17191D"));

    public IBrush BubbleBorderBrush =>
        IsMine
            ? new SolidColorBrush(
                Color.Parse("#3474F2"))
            : new SolidColorBrush(
                Color.Parse("#20242C"));

    public BoxShadows BubbleShadow =>
        IsMine
            ? new BoxShadows(
                new BoxShadow
                {
                    OffsetX = 0,
                    OffsetY = 3,
                    Blur = 18,
                    Spread = 0,
                    Color = Color.Parse("#402563EB")
                })
            : new BoxShadows();

    public IBrush BubbleForeground =>
        new SolidColorBrush(
            Color.Parse("#F8FAFC"));

    public IBrush MetaForeground =>
        IsMine
            ? new SolidColorBrush(
                Color.Parse("#9DB8F5"))
            : new SolidColorBrush(
                Color.Parse("#53627D"));

    public string TimeText =>
        SentAt.ToString("h:mm tt");

    public string EditedText =>
        IsEdited
            ? "edited"
            : string.Empty;

    private static string GetInitials(
        string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "?";

        var parts =
            name.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
        {
            return parts[0][..1]
                .ToUpperInvariant();
        }

        return
            $"{parts[0][0]}{parts[^1][0]}"
                .ToUpperInvariant();
    }

    private static string GetAvatarBrush(
        string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "#2563EB";

        var brushes =
            new[]
            {
                "#2563EB",
                "#8B5CF6",
                "#10B981",
                "#EC4899",
                "#F97316",
                "#06B6D4",
                "#6366F1",
                "#3B82F6"
            };

        var hash =
            Math.Abs(name.GetHashCode());

        return brushes[
            hash % brushes.Length];
    }
}