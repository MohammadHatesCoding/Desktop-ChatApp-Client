using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.ViewModels.Auth;

public sealed class CreateAccountViewModel : ViewModelBase
{
    private string _fullName = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;

    private string _fullNameError = string.Empty;
    private string _emailError = string.Empty;
    private string _passwordError = string.Empty;
    private string _confirmPasswordError = string.Empty;

    private string _generalError = string.Empty;

    private bool _isLoading;

    public CreateAccountViewModel()
    {
        CreateAccountCommand =
            new AsyncRelayCommand(CreateAccountAsync);

        NavigateToLoginCommand =
            new RelayCommand(NavigateToLogin);
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (SetProperty(ref _fullName, value))
            {
                FullNameError = string.Empty;
            }
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (SetProperty(ref _email, value))
            {
                EmailError = string.Empty;
            }
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (SetProperty(ref _password, value))
            {
                PasswordError = string.Empty;
                ConfirmPasswordError = string.Empty;
            }
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            if (SetProperty(ref _confirmPassword, value))
            {
                ConfirmPasswordError = string.Empty;
            }
        }
    }

    public string FullNameError
    {
        get => _fullNameError;
        private set => SetProperty(ref _fullNameError, value);
    }

    public string EmailError
    {
        get => _emailError;
        private set => SetProperty(ref _emailError, value);
    }

    public string PasswordError
    {
        get => _passwordError;
        private set => SetProperty(ref _passwordError, value);
    }

    public string ConfirmPasswordError
    {
        get => _confirmPasswordError;
        private set => SetProperty(
            ref _confirmPasswordError,
            value);
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

            // -------------------------------------------------
            // TODO:
            // اینجا در مرحله بعدی Register Use Case را صدا
            // خواهیم زد و آن را به IAuthService متصل می‌کنیم.
            //
            // فعلاً فقط UI و Validation را کامل می‌کنیم.
            // -------------------------------------------------

            await Task.Delay(500);
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

        FullNameError = string.Empty;
        EmailError = string.Empty;
        PasswordError = string.Empty;
        ConfirmPasswordError = string.Empty;

        if (string.IsNullOrWhiteSpace(FullName))
        {
            FullNameError = "Full name is required.";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            EmailError = "Email address is required.";
            isValid = false;
        }
        else if (!IsValidEmail(Email))
        {
            EmailError = "Please enter a valid email address.";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            PasswordError = "Password is required.";
            isValid = false;
        }
        else if (Password.Length < 8)
        {
            PasswordError =
                "Password must contain at least 8 characters.";

            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ConfirmPasswordError =
                "Please repeat your password.";

            isValid = false;
        }
        else if (Password != ConfirmPassword)
        {
            ConfirmPasswordError =
                "Passwords do not match.";

            isValid = false;
        }

        return isValid;
    }

    private static bool IsValidEmail(string email)
    {
        return email.Contains('@') &&
               email.Contains('.') &&
               email.IndexOf('@') > 0 &&
               email.IndexOf('@') < email.Length - 1;
    }

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