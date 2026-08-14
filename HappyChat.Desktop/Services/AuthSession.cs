using HappyChat.Application.Interfaces;
using System;

namespace HappyChat.Infrastructure.Auth;

public sealed class AuthSession : IAuthSession
{
    public string? PhoneNumber { get; private set; }

    public string? AccessToken { get; private set; }

    public string? RefreshToken { get; private set; }

    public DateTime? AccessTokenExpiresAt { get; private set; }

    public void SetPhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }

    public void SetTokens(
        string accessToken,
        string refreshToken,
        DateTime accessTokenExpiresAt)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        AccessTokenExpiresAt = accessTokenExpiresAt;
    }

    public void Clear()
    {
        PhoneNumber = null;
        AccessToken = null;
        RefreshToken = null;
        AccessTokenExpiresAt = null;
    }
}