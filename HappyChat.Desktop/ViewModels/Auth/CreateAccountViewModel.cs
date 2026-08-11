using HappyChat.Application.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.ViewModels.Auth;

public sealed class CreateAccountViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
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

    public CreateAccountViewModel(IAuthService authService)
    {
        _authService = authService;

        CreateAccountCommand =
            new AsyncRelayCommand(CreateAccountAsync);

        NavigateToLoginCommand =
            new RelayCommand(NavigateToLogin);
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
        get => _lastNameError;
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

    public ICommand NavigateToLoginCommand { get; }

    private async Task CreateAccountAsync()
    {
        GeneralError = string.Empty;

        if (!Validate())
            return;

        try
        {
            IsLoading = true;

            var result = await _authService
                .RegisterAsync(Name, LastName, DateTime.Parse(BirthDate), PhoneNumber);

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
            LastNameError = "Last is required.";
            isValid = false;
        }
        else if (string.IsNullOrWhiteSpace(BirthDate))
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

    //private static bool IsValidEmail(string email)
    //{
    //    return email.Contains('@') &&
    //           email.Contains('.') &&
    //           email.IndexOf('@') > 0 &&
    //           email.IndexOf('@') < email.Length - 1;
    //}

    private void NavigateToLogin()
    {
        // Navigation Service در مرحله بعدی اینجا قرار می‌گیرد.
    }
}


// ---------------------------------------------------------
// Simple ICommand
// ---------------------------------------------------------

public sealed class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(
        Action execute,
        Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(
            this,
            EventArgs.Empty);
    }
}


// ---------------------------------------------------------
// Async ICommand
// ---------------------------------------------------------

public sealed class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<Task> execute)
    {
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return !_isExecuting;
    }

    public async void Execute(object? parameter)
    {
        if (_isExecuting)
            return;

        try
        {
            _isExecuting = true;

            RaiseCanExecuteChanged();

            await _execute();
        }
        finally
        {
            _isExecuting = false;

            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(
            this,
            EventArgs.Empty);
    }
}