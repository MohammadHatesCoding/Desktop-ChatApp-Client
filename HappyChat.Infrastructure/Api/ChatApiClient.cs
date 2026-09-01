using HappyChat.Application.DTOs.Chat;
using HappyChat.Application.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HappyChat.Infrastructure.Api;

public sealed class ChatApiClient : IChatService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthSession _authSession;

    public ChatApiClient(HttpClient httpClient, IAuthSession authSession)
    {
        _httpClient = httpClient;
        _authSession = authSession;
    }

    public async Task<IReadOnlyList<GetAllChatsResponse>> GetAllChatsAsync(CancellationToken cancellationToken = default)
    {
        using var request = CreateAuthenticatedRequest(HttpMethod.Get, "Chat/GetAllChats");

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<List<GetAllChatsResponse>>(cancellationToken);

        return result ?? [];
    }

    public async Task<OpenChatResponse?> OpenChatAsync(int chatId, CancellationToken cancellationToken = default)
    {
        using var request = CreateAuthenticatedRequest(HttpMethod.Get, $"Chat/OpenChat?query.ChatId={chatId}");

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<OpenChatResponse>(cancellationToken);

        return result;
    }

    public async Task<IReadOnlyList<GetMessagesResponse>> GetMessagesAsync(int chatId, int page = 1, int pageSize = 30, CancellationToken cancellationToken = default)
    {
        using var request = CreateAuthenticatedRequest(HttpMethod.Get, $"Message/GetMessages?Query.ChatId={chatId}&Query.Page={page}&Query.PageSize={pageSize}");

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<List<GetMessagesResponse>>(cancellationToken);

        return result ?? [];
    }

    private HttpRequestMessage CreateAuthenticatedRequest(HttpMethod method, string url)
    {
        var request = new HttpRequestMessage(method, url);

        var accessToken = _authSession.AccessToken;

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new InvalidOperationException("Access token is missing. Please authenticate first.");
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return request;
    }
}