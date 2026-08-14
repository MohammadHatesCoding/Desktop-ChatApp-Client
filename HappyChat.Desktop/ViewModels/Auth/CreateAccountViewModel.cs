using HappyChat.Application.Interfaces;
using HappyChat.Desktop.Commands;
using HappyChat.Desktop.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.ViewModels.Auth;

public sealed class CreateAccountViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IAuthSession _authSession;
    private string _name = string.Empty;
    private string _lastName = string.Empty;
    private string _birthDate = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _nameError = string.Empty;
    private string _lastNameError = string.Empty;
    private string _birthDateError = string.Empty;
    private string _phoneNumberError = string.Empty;
    private string _generalError = string.Empty;
    private bool _isLoading;

    public CreateAccountViewModel(IAuthService authService, INavigationService navigationService, IAuthSession authSession)
    {
        _authService = authService;
        
        _navigationService = navigationService;

        _authSession = authSession;

        CreateAccountCommand = new AsyncRelayCommand(CreateAccountAsync);

        NavigateToSignInCommand = new RelayCommand(NavigateToSignIn);
    }

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                NameError = string.Empty;
            }
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (SetProperty(ref _lastName, value))
            {
                LastNameError = string.Empty;
            }
        }
    }

    public string BirthDate
    {
        get => _birthDate;
        set
        {
            if (SetProperty(ref _birthDate, value))
            {
                BirthDateError = string.Empty;
            }
        }
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

    public string NameError
    {
        get => _nameError;
        private set => SetProperty(ref _nameError, value);
    }

    public string LastNameError
    {
        get => _lastNameError;
        private set => SetProperty(ref _lastNameError, value);
    }

    public string BirthDateError
    {
        get => _birthDateError;
        private set => SetProperty(ref _birthDateError, value);
    }

    public string PhoneNumberError
    {
        get => _phoneNumberError;
        private set => SetProperty(ref _phoneNumberError, value);
    }

    public string GeneralError
    {
        get => _generalError;
        private set => SetProperty(
            ref _generalError,
            value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (SetProperty(ref _isLoading, value))
            {
                if (CreateAccountCommand is AsyncRelayCommand command)
                    command.RaiseCanExecuteChanged();
            }
        }
    }

    public ICommand CreateAccountCommand { get; }

    public ICommand NavigateToSignInCommand { get; }

    private async Task CreateAccountAsync()
    {
        GeneralError = string.Empty;

        if (!Validate())
            return;

        try
        {
            IsLoading = true;

            var result = await _authService.RegisterAsync(Name, LastName, DateTime.Parse(BirthDate), PhoneNumber);

            if (result)
            {
                _authSession.SetPhoneNumber(PhoneNumber);

                _navigationService.NavigateTo<VerifyOtpViewModel>();
            }
        }
        catch (Exception)
        {
            GeneralError =
                "Something went wrong. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool Validate()
    {
        bool isValid = true;

        NameError = string.Empty;
        LastNameError = string.Empty;
        BirthDateError = string.Empty;
        PhoneNumberError = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            NameError = "Name is required.";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(LastName))
        {
            LastNameError = "Last name is required.";
            isValid = false;
        }
        
        if (string.IsNullOrWhiteSpace(BirthDate))
        {
            BirthDateError = "Please enter a valid birth date.";
            isValid = false;
        }

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

    private void NavigateToSignIn()
    {
        _navigationService.NavigateTo<LoginViewModel>();
    }
}