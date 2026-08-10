using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace HappyChat.Desktop.Controls.Forms;

public partial class PasswordField : UserControl
{
    // ---------------------------------------------------------
    // Label
    // ---------------------------------------------------------

    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<PasswordField, string>(
            nameof(Label),
            string.Empty);

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }


    // ---------------------------------------------------------
    // Placeholder
    // ---------------------------------------------------------

    public static readonly StyledProperty<string> PlaceholderProperty =
        AvaloniaProperty.Register<PasswordField, string>(
            nameof(Placeholder),
            string.Empty);

    public string Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }


    // ---------------------------------------------------------
    // Text
    // ---------------------------------------------------------

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<PasswordField, string>(
            nameof(Text),
            string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    // ---------------------------------------------------------
    // Error
    // ---------------------------------------------------------

    public static readonly StyledProperty<string> ErrorProperty =
        AvaloniaProperty.Register<PasswordField, string>(
            nameof(Error),
            string.Empty);

    public string Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }


    // ---------------------------------------------------------
    // Password Visibility
    // ---------------------------------------------------------

    public static readonly StyledProperty<bool> IsPasswordVisibleProperty =
        AvaloniaProperty.Register<PasswordField, bool>(
            nameof(IsPasswordVisible),
            false);

    public bool IsPasswordVisible
    {
        get => GetValue(IsPasswordVisibleProperty);
        set => SetValue(IsPasswordVisibleProperty, value);
    }


    // ---------------------------------------------------------
    // Has Error
    // ---------------------------------------------------------

    public static readonly DirectProperty<PasswordField, bool> HasErrorProperty =
        AvaloniaProperty.RegisterDirect<PasswordField, bool>(
            nameof(HasError),
            o => o.HasError);

    private bool _hasError;

    public bool HasError
    {
        get => _hasError;

        private set =>
            SetAndRaise(
                HasErrorProperty,
                ref _hasError,
                value);
    }


    // ---------------------------------------------------------
    // Constructor
    // ---------------------------------------------------------

    public PasswordField()
    {
        InitializeComponent();

        ToggleVisibilityCommand =
            new TogglePasswordCommand(this);

        UpdateHasError();

        UpdatePasswordMode();
    }


    // ---------------------------------------------------------
    // Property Changed
    // ---------------------------------------------------------

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ErrorProperty)
        {
            UpdateHasError();
        }

        if (change.Property == IsPasswordVisibleProperty)
        {
            UpdatePasswordMode();
        }
    }


    // ---------------------------------------------------------
    // Error State
    // ---------------------------------------------------------

    private void UpdateHasError()
    {
        HasError =
            !string.IsNullOrWhiteSpace(Error);
    }


    // ---------------------------------------------------------
    // Password Mode
    // ---------------------------------------------------------

    private void UpdatePasswordMode()
    {
        var passwordBox =
            this.FindControl<TextBox>("PasswordBox");

        if (passwordBox is null)
            return;

        passwordBox.PasswordChar =
            IsPasswordVisible
                ? '\0'
                : '●';
    }


    // ---------------------------------------------------------
    // Toggle Command
    // ---------------------------------------------------------

    public ICommand ToggleVisibilityCommand { get; }


    private sealed class TogglePasswordCommand : ICommand
    {
        private readonly PasswordField _owner;

        public TogglePasswordCommand(
            PasswordField owner)
        {
            _owner = owner;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _owner.IsPasswordVisible =
                !_owner.IsPasswordVisible;
        }
    }
}