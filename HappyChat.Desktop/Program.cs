using Avalonia;
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

        services.AddTransient<
            Views.MainWindow>();

        services.AddTransient<
            ViewModels.Auth.CreateAccountViewModel>();

        Services = services.BuildServiceProvider();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
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