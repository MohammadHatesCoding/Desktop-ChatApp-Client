namespace HappyChat.Application.Interfaces;

public interface IAuthSession
{
    string? PhoneNumber { get; }

    string? AccessToken { get; }
    string? RefreshToken { get; }

    DateTime? AccessTokenExpiresAt { get; }

    void SetPhoneNumber(string phoneNumber);

    void SetTokens(string accessToken, string refreshToken, DateTime accessTokenExpiresAt);

    void Clear();
}