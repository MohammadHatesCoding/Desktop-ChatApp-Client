using HappyChat.Application.DTOs.Chat;

namespace HappyChat.Application.Interfaces;

public interface IChatService
{
    Task<IReadOnlyList<GetAllChatsResponse>> GetAllChatsAsync(CancellationToken cancellationToken = default);

    Task<OpenChatResponse?> OpenChatAsync(int chatId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<GetMessagesResponse>> GetMessagesAsync(int chatId, int page = 1, 
        int pageSize = 30, CancellationToken cancellationToken = default);
}