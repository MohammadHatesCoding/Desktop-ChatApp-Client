using System.Collections.Generic;
using System.Threading.Tasks;
using HappyChat.Application.DTOs;

namespace HappyChat.Application.Interfaces;

public interface IChatService
{
    Task<List<ChatSummaryDto>> GetChatsAsync();

    Task<List<ChatMessageDto>> GetMessagesAsync(int chatId);

    Task<ChatMessageDto?> SendMessageAsync(int chatId, string content);
}