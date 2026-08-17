using HappyChat.Application.DTOs;
using HappyChat.Application.Interfaces;
using System.Net.Http.Json;

namespace HappyChat.Infrastructure.Services;

public sealed class ChatApiClient : IChatService
{
    private readonly HttpClient _httpClient;

    public ChatApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ChatSummaryDto>> GetChatsAsync()
    {
        var result =
            await _httpClient.GetFromJsonAsync<List<ChatSummaryDto>>("chats");

        return result ?? new List<ChatSummaryDto>();
    }

    public async Task<List<ChatMessageDto>> GetMessagesAsync(int chatId)
    {
        var result =
            await _httpClient.GetFromJsonAsync<List<ChatMessageDto>>($"chats/{chatId}/messages");

        return result ?? new List<ChatMessageDto>();
    }

    public async Task<ChatMessageDto?> SendMessageAsync(int chatId, string content)
    {
        var response =
            await _httpClient.PostAsJsonAsync($"chats/{chatId}/messages", new { content });

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ChatMessageDto>();
    }
}