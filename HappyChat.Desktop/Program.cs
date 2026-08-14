using Avalonia;
using HappyChat.Application.Interfaces;
using HappyChat.Desktop.Services;
using HappyChat.Desktop.ViewModels.Auth;
using HappyChat.Infrastructure.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HappyChat.Desktop;

internal static class Program
{
    public static IServiceProvider Services { get; private set; } = null!;

    [STAThread]
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddInfrastructure();

        services.AddSingleton<INavigationService, NavigationService>();

        services.AddSingleton<IAuthSession, AuthSession>();

        services.AddTransient<Views.MainWindow>();

        services.AddTransient<CreateAccountViewModel>();
        
        services.AddTransient<LoginViewModel>();

        services.AddTransient<LoginViewModel>();

        services.AddTransient<VerifyOtpViewModel>();

        Services = services.BuildServiceProvider();

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}