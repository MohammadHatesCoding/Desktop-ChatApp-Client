namespace HappyChat.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(string name, string lastName, DateTime birthDate, string phone);
}
