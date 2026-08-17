using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace HappyChat.Desktop.Controls.Chat;

public partial class MessageInputBar : UserControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<MessageInputBar, string>(nameof(Text), string.Empty);

    public static readonly StyledProperty<ICommand?> SendCommandProperty =
        AvaloniaProperty.Register<MessageInputBar, ICommand?>(nameof(SendCommand));

    public static readonly StyledProperty<ICommand?> AttachCommandProperty =
        AvaloniaProperty.Register<MessageInputBar, ICommand?>(nameof(AttachCommand));

    public MessageInputBar()
    {
        InitializeComponent();
    }

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public ICommand? SendCommand
    {
        get => GetValue(SendCommandProperty);
        set => SetValue(SendCommandProperty, value);
    }

    public ICommand? AttachCommand
    {
        get => GetValue(AttachCommandProperty);
        set => SetValue(AttachCommandProperty, value);
    }
}