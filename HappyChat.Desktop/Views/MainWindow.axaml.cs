using Avalonia.Controls;
using HappyChat.Desktop.Services;
using HappyChat.Desktop.ViewModels;
using HappyChat.Desktop.ViewModels.Auth;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HappyChat.Desktop.Views;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly INavigationService _navigationService;

    private ViewModelBase _currentViewModel;

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;

        private set
        {
            if (_currentViewModel == value)
                return;

            _currentViewModel = value;

            OnPropertyChanged();
        }
    }

    public MainWindow(
        CreateAccountViewModel initialViewModel,
        INavigationService navigationService)
    {
        InitializeComponent();

        _navigationService = navigationService;

        _currentViewModel = initialViewModel;

        _navigationService.NavigationChanged +=
            OnNavigationChanged;

        DataContext = this;
    }

    private void OnNavigationChanged(
        ViewModelBase viewModel)
    {
        CurrentViewModel = viewModel;
    }

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler?
        PropertyChanged;

    protected override void OnClosed(EventArgs e)
    {
        _navigationService.NavigationChanged -=
            OnNavigationChanged;

        base.OnClosed(e);
    }
}