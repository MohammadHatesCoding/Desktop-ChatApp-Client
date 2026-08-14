using HappyChat.Application.Interfaces;
using HappyChat.Desktop.Commands;
using HappyChat.Desktop.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.ViewModels.Auth;

public sealed class LoginViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IAuthSession _authSession;
    private string _phoneNumber = string.Empty;
    private string _phoneNumberError = string.Empty;
    private string _generalError = string.Empty;
    private bool _isLoading;

    public LoginViewModel(IAuthService authService, INavigationService navigationService, IAuthSession authSession)
    {
        _authService = authService;

        _navigationService = navigationService;

        _authSession = authSession;

        LoginCommand = new AsyncRelayCommand(LoginAsync);

        NavigateToSignUpCommand = new RelayCommand(NavigateToSignUp);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (SetProperty(ref _phoneNumber, value))
            {
                PhoneNumberError = string.Empty;
            }
        }
    }

    public string PhoneNumberError
    {
        get => _phoneNumberError;
        private set => SetProperty(ref _phoneNumberError, value);
    }

    public string GeneralError
    {
        get => _generalError;
        private set => SetProperty(ref _generalError, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (SetProperty(ref _isLoading, value))
            {
                if (LoginCommand is AsyncRelayCommand command)
                    command.RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand LoginCommand { get; }

    public ICommand NavigateToSignUpCommand { get; }

    private async Task LoginAsync()
    {
        GeneralError = string.Empty;

        if (!Validate())
            return;

        try
        {
            IsLoading = true;

            var result = await _authService.LoginAsync(PhoneNumber);


            if (result)
            {
                _authSession.SetPhoneNumber(PhoneNumber);

                _navigationService.NavigateTo<VerifyOtpViewModel>();
            }

        }
        catch (Exception)
        {
            GeneralError =
                "Unable to sign in. Please check your phone number.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool Validate()
    {
        bool isValid = true;

        PhoneNumberError = string.Empty;

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            PhoneNumberError = "PhoneNumber is required.";
            isValid = false;
        }
        else if (PhoneNumber.Length < 11 || 13 < PhoneNumber.Length)
        {
            PhoneNumberError = "PhoneNumber must contain at least 8 characters.";

            isValid = false;
        }

        return isValid;
    }

    private void NavigateToSignUp()
    {
        _navigationService.NavigateTo<CreateAccountViewModel>();
    }
}