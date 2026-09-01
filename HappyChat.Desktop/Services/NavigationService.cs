using HappyChat.Desktop.ViewModels;
using HappyChat.Desktop.ViewModels.Chat;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace HappyChat.Desktop.Services;

public interface INavigationService
{
    event Action<ViewModelBase>? NavigationChanged;

    Task NavigateTo<TViewModel>()
        where TViewModel : ViewModelBase;
}


public sealed class NavigationService : INavigationService
{
    private readonly IServiceProvider _services;

    public event Action<ViewModelBase>? NavigationChanged;


    public NavigationService(IServiceProvider services)
    {
        _services = services;
    }


    public async Task NavigateTo<TViewModel>()
        where TViewModel : ViewModelBase
    {
        var viewModel =
            _services.GetRequiredService<TViewModel>();


        if (viewModel is ChatViewModel chatViewModel)
        {
            await chatViewModel.InitializeAsync();
        }


        NavigationChanged?.Invoke(viewModel);
    }
}