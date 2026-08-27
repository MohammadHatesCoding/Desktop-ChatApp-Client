using HappyChat.Application.DTOs;
using HappyChat.Application.Interfaces;
using System.Net.Http.Json;

namespace HappyChat.Infrastructure.Api;

public class AuthApiClient : IAuthService
{
    private readonly HttpClient _http;


    public AuthApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> RegisterAsync(string name, string lastName, DateTime birthDate, string phoneNumber)
    {
        var response = await _http.PostAsJsonAsync("User/Register", new { command = new { name, lastName, birthDate, phoneNumber } });

        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> LoginAsync(string phoneNumber)
    {
        var response = await _http.PostAsJsonAsync("User/Login", new { command = new { phoneNumber } });

        var content = await response.Content.ReadAsStringAsync();

        return response.IsSuccessStatusCode;
    }

    public async Task<CheckOTPResponse?> CheckOTPAsync(string phoneNumber, string otp)
    {
        var response = await _http.PostAsJsonAsync("User/CheckOtp", new { command = new { phoneNumber, otp } });

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<CheckOTPResponse>();
    }
}