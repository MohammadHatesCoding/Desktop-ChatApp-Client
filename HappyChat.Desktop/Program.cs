//using Avalonia;
//using HappyChat.Desktop.DependencyInjection;
//using HappyChat.Desktop;
//using Microsoft.Extensions.DependencyInjection;
//using System;

//namespace HappyChat.Desktop;

//sealed class Program
//{
//    public static IServiceProvider Services { get; private set; } = null!;



//    [STAThread]
//    public static void Main(string[] args)
//    {
//        var services = new ServiceCollection();

//        services.AddDesktopServices();
//        services.AddInfrastructure();

//        Services = services.BuildServiceProvider();

//        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
//    }

//    public static AppBuilder BuildAvaloniaApp()
//        => AppBuilder.Configure<App>()
//            .UsePlatformDetect()
//#if DEBUG
//            .WithDeveloperTools()
//#endif
//            .WithInterFont()
//            .LogToTrace();
//}


using Avalonia;
using System;

namespace HappyChat.Desktop;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
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