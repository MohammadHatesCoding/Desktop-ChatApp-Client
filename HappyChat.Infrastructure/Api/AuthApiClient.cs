using HappyChat.Application.Interfaces;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Xml.Linq;

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
}