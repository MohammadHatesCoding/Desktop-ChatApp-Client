using HappyChat.Application.DTOs.Chat;
using HappyChat.Shared.Enum;

namespace HappyChat.Application.Interfaces;

public interface IMessageService
{
    Task SendMessage(int? ChatId, int? ReceiverUserId, string Content, int? RepliedTo, CancellationToken cancellationToken = default);

    Task EditMessage(int MessageId, string Content, CancellationToken cancellationToken = default);

    Task DeleteMessage(int MessageId, DeleteType DeleteType, CancellationToken cancellationToken = default);
}