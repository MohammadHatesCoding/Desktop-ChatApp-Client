using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace HappyChat.Desktop.Controls.Common;

public partial class PrimaryButton : UserControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<PrimaryButton, string>(
            nameof(Text),
            "Button");

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<PrimaryButton, ICommand?>(
            nameof(Command));

    public PrimaryButton()
    {
        InitializeComponent();
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}