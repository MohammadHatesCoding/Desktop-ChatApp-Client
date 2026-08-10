using Microsoft.Extensions.DependencyInjection;

namespace HappyChat.Desktop.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDesktopServices(
        this IServiceCollection services)
    {
        return services;
    }
}