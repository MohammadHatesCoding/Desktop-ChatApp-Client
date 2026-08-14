using HappyChat.Application.DTOs;

namespace HappyChat.Application.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(string name, string lastName, DateTime birthDate, string phone);
    Task<bool> LoginAsync(string phoneNumber);

    Task<CheckOTPResponse?> CheckOTPAsync(string phoneNumber, string otp);
}
