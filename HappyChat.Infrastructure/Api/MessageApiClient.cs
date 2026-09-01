using HappyChat.Application.DTOs.Chat;
using HappyChat.Application.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace HappyChat.Infrastructure.Api;

public sealed class MessageApiClient : IMessageService
{
    private readonly HttpClient _httpClient;
    private readonly IAuthSession _authSession;
    public MessageApiClient(HttpClient httpClient, IAuthSession authSession)
    {
        _httpClient = httpClient;
        _authSession = authSession;
    }

    public async Task SendMessage(int? ChatId, int? ReceiverUserId, string Content, int? RepliedTo, CancellationToken cancellationToken = default)
    {
        using var request = CreateAuthenticatedRequest(HttpMethod.Post, "Message/SendMessage");

        request.Content = JsonContent.Create(new
        {
            command = new
            {
                chatId = ChatId,
                receiverUserId = ReceiverUserId,
                content = Content,
                repliedTo = RepliedTo
            }
        });

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();
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