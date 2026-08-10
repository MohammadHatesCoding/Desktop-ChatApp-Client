using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace HappyChat.Desktop.Controls.Forms;

public partial class FormField : UserControl
{
    // =========================================================
    // Label
    // =========================================================

    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<FormField, string>(
            nameof(Label),
            string.Empty);

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }


    // =========================================================
    // Placeholder
    // =========================================================

    public static readonly StyledProperty<string> PlaceholderProperty =
        AvaloniaProperty.Register<FormField, string>(
            nameof(Placeholder),
            string.Empty);

    public string Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }


    // =========================================================
    // Text
    // =========================================================

    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<FormField, string>(
            nameof(Text),
            string.Empty,
            defaultBindingMode: BindingMode.TwoWay);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    // =========================================================
    // Error
    // =========================================================

    public static readonly StyledProperty<string> ErrorProperty =
        AvaloniaProperty.Register<FormField, string>(
            nameof(Error),
            string.Empty);

    public string Error
    {
        get => GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }


    // =========================================================
    // Has Error
    // =========================================================

    public static readonly DirectProperty<FormField, bool> HasErrorProperty =
        AvaloniaProperty.RegisterDirect<FormField, bool>(
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


    // =========================================================
    // Constructor
    // =========================================================

    public FormField()
    {
        InitializeComponent();

        UpdateHasError();
    }


    // =========================================================
    // Property Changed
    // =========================================================

    protected override void OnPropertyChanged(
        AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ErrorProperty)
        {
            UpdateHasError();
        }
    }


    // =========================================================
    // Error State
    // =========================================================

    private void UpdateHasError()
    {
        HasError =
            !string.IsNullOrWhiteSpace(Error);
    }
}