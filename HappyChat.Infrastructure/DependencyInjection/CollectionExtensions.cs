using HappyChat.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddHttpClient<IAuthService, AuthApiClient>(x => {x.BaseAddress = new Uri("https://localhost:7271/api/");});

        return services;
    }
}