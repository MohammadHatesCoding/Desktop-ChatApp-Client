using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HappyChat.Desktop.Controls.Chat;

public partial class ChatHeader : UserControl
{
    public static readonly StyledProperty<string> InitialsProperty =
        AvaloniaProperty.Register<ChatHeader, string>(nameof(Initials), string.Empty);

    public static readonly StyledProperty<IBrush> AvatarBrushProperty =
        AvaloniaProperty.Register<ChatHeader, IBrush>(nameof(AvatarBrush), Brushes.SlateGray);

    public static readonly StyledProperty<bool> IsOnlineProperty =
        AvaloniaProperty.Register<ChatHeader, bool>(nameof(IsOnline), false);

    public static readonly StyledProperty<string> NameProperty =
        AvaloniaProperty.Register<ChatHeader, string>(nameof(Name), string.Empty);

    public static readonly StyledProperty<string> StatusTextProperty =
        AvaloniaProperty.Register<ChatHeader, string>(nameof(StatusText), string.Empty);

    public static readonly StyledProperty<ICommand?> CallCommandProperty =
        AvaloniaProperty.Register<ChatHeader, ICommand?>(nameof(CallCommand));

    public static readonly StyledProperty<ICommand?> VideoCallCommandProperty =
        AvaloniaProperty.Register<ChatHeader, ICommand?>(nameof(VideoCallCommand));

    public static readonly StyledProperty<ICommand?> SearchCommandProperty =
        AvaloniaProperty.Register<ChatHeader, ICommand?>(nameof(SearchCommand));

    public static readonly StyledProperty<ICommand?> MoreCommandProperty =
        AvaloniaProperty.Register<ChatHeader, ICommand?>(nameof(MoreCommand));

    public ChatHeader()
    {
        InitializeComponent();
    }

    public string Initials
    {
        get => GetValue(InitialsProperty);
        set => SetValue(InitialsProperty, value);
    }

    public IBrush AvatarBrush
    {
        get => GetValue(AvatarBrushProperty);
        set => SetValue(AvatarBrushProperty, value);
    }

    public bool IsOnline
    {
        get => GetValue(IsOnlineProperty);
        set => SetValue(IsOnlineProperty, value);
    }

    public string Name
    {
        get => GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public string StatusText
    {
        get => GetValue(StatusTextProperty);
        set => SetValue(StatusTextProperty, value);
    }

    public ICommand? CallCommand
    {
        get => GetValue(CallCommandProperty);
        set => SetValue(CallCommandProperty, value);
    }

    public ICommand? VideoCallCommand
    {
        get => GetValue(VideoCallCommandProperty);
        set => SetValue(VideoCallCommandProperty, value);
    }

    public ICommand? SearchCommand
    {
        get => GetValue(SearchCommandProperty);
        set => SetValue(SearchCommandProperty, value);
    }

    public ICommand? MoreCommand
    {
        get => GetValue(MoreCommandProperty);
        set => SetValue(MoreCommandProperty, value);
    }
}