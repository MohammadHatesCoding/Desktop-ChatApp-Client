using Avalonia.Threading;
using HappyChat.Application.DTOs;
using HappyChat.Application.Interfaces;
using HappyChat.Desktop.Commands;
using HappyChat.Desktop.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.ViewModels.Auth;

public sealed class VerifyOtpViewModel : ViewModelBase
{
    private readonly IAuthService _authService;
    private readonly IAuthSession _authSession;
    private readonly INavigationService _navigationService;

    private string _digit1 = string.Empty;
    private string _digit2 = string.Empty;
    private string _digit3 = string.Empty;
    private string _digit4 = string.Empty;
    private string _digit5 = string.Empty;
    private string _digit6 = string.Empty;

    private string _otpError = string.Empty;

    private int _remainingSeconds = 60;

    private readonly DispatcherTimer _countdownTimer;

    private bool _isLoading;

    public VerifyOtpViewModel(IAuthService authService, IAuthSession authSession, INavigationService navigationService)
    {
        _authService = authService;
        _authSession = authSession;
        _navigationService = navigationService;

        VerifyCommand = new AsyncRelayCommand(VerifyAsync);

        BackToLoginCommand = new RelayCommand(BackToLogin);

        _countdownTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        _countdownTimer.Tick += CountdownTimer_Tick;

        _countdownTimer.Start();
    }

    // =========================================================
    // Phone Number
    // =========================================================

    public string PhoneNumber =>
        _authSession.PhoneNumber ?? string.Empty;


    // =========================================================
    // OTP Digits
    // =========================================================

    public string Digit1
    {
        get => _digit1;
        set => SetProperty(ref _digit1, value);
    }

    public string Digit2
    {
        get => _digit2;
        set => SetProperty(ref _digit2, value);
    }

    public string Digit3
    {
        get => _digit3;
        set => SetProperty(ref _digit3, value);
    }

    public string Digit4
    {
        get => _digit4;
        set => SetProperty(ref _digit4, value);
    }

    public string Digit5
    {
        get => _digit5;
        set => SetProperty(ref _digit5, value);
    }

    public string Digit6
    {
        get => _digit6;
        set => SetProperty(ref _digit6, value);
    }


    // =========================================================
    // Error
    // =========================================================

    public string OtpError
    {
        get => _otpError;
        private set => SetProperty(ref _otpError, value);
    }


    // =========================================================
    // Countdown
    // =========================================================

    public int RemainingSeconds
    {
        get => _remainingSeconds;
        private set
        {
            if (SetProperty(ref _remainingSeconds, value))
            {
                OnPropertyChanged(nameof(CountdownText));
                OnPropertyChanged(nameof(CanResend));
            }
        }
    }

    public string CountdownText =>
        $"Resend code in 0:{RemainingSeconds:00}";

    public bool CanResend =>
        RemainingSeconds <= 0;

    private void CountdownTimer_Tick(object? sender, EventArgs e)
    {
        if (RemainingSeconds > 0)
        {
            RemainingSeconds--;
        }

        if (RemainingSeconds <= 0)
        {
            _countdownTimer.Stop();
        }
    }

    public void StopCountdown()
    {
        _countdownTimer.Stop();
    }


    // =========================================================
    // Loading
    // =========================================================

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (SetProperty(ref _isLoading, value))
            {
                if (VerifyCommand is AsyncRelayCommand command)
                    command.RaiseCanExecuteChanged();
            }
        }
    }


    // =========================================================
    // Commands
    // =========================================================

    public ICommand VerifyCommand { get; }

    public ICommand BackToLoginCommand { get; }


    // =========================================================
    // Verify
    // =========================================================

    private async Task VerifyAsync()
    {
        OtpError = string.Empty;

        string otp =
            Digit1 +
            Digit2 +
            Digit3 +
            Digit4 +
            Digit5 +
            Digit6;

        if (otp.Length != 6)
        {
            OtpError = "Please enter the 6-digit verification code.";
            return;
        }

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            OtpError = "Phone number is missing. Please go back and try again.";
            return;
        }

        try
        {
            IsLoading = true;

            var result =
                await _authService.CheckOTPAsync(PhoneNumber, otp);

            if (result is null)
            {
                OtpError = "The verification code is invalid.";
                return;
            }

            // Save authentication information
            _authSession.SetTokens(
                result.AccessToken,
                result.RefreshToken,
                result.AccessTokenExpiresAt);

            StopCountdown();

            // مرحله بعدی:
            // Navigate to Chat/Main application
        }
        catch (Exception)
        {
            OtpError =
                "Something went wrong. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }


    // =========================================================
    // Back
    // =========================================================

    private void BackToLogin()
    {
        StopCountdown();
        _navigationService.NavigateTo<LoginViewModel>();
    }
}