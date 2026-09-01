using HappyChat.Application.DTOs.Chat;

namespace HappyChat.Application.Interfaces;

public interface IMessageService
{
    Task SendMessage(int? ChatId, int? ReceiverUserId, string Content, int? RepliedTo, CancellationToken cancellationToken = default);
}