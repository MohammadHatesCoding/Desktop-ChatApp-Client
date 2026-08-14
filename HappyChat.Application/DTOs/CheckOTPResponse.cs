namespace HappyChat.Application.DTOs;

public sealed record CheckOTPResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresAt);