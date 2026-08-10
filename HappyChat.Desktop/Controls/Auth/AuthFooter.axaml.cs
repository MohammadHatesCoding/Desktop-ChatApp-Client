using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace HappyChat.Desktop.Controls.Auth;

public partial class AuthFooter : UserControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<AuthFooter, string>(
            nameof(Text),
            string.Empty);

    public static readonly StyledProperty<string> LinkTextProperty =
        AvaloniaProperty.Register<AuthFooter, string>(
            nameof(LinkText),
            string.Empty);

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<AuthFooter, ICommand?>(
            nameof(Command));

    public AuthFooter()
    {
        InitializeComponent();
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string LinkText
    {
        get => GetValue(LinkTextProperty);
        set => SetValue(LinkTextProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}