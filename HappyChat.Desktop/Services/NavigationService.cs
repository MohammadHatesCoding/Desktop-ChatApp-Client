using HappyChat.Application.Interfaces;
using HappyChat.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HappyChat.Desktop.Services;

public interface INavigationService
{
    event Action<ViewModelBase>? NavigationChanged;

    void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
}
public sealed class NavigationService : INavigationService
{
    private readonly IServiceProvider _services;

    public event Action<ViewModelBase>? NavigationChanged;

    public NavigationService(IServiceProvider services)
    {
        _services = services;
    }

    public void NavigateTo<TViewModel>()
        where TViewModel : ViewModelBase
    {
        var viewModel =
            _services.GetRequiredService<TViewModel>();

        NavigationChanged?.Invoke(viewModel);
    }
}