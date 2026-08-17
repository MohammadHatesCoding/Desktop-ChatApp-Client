using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HappyChat.Desktop.Controls.Chat;

public partial class ChatListItem : UserControl
{
    public static readonly StyledProperty<string> InitialsProperty =
        AvaloniaProperty.Register<ChatListItem, string>(nameof(Initials), string.Empty);

    public static readonly StyledProperty<IBrush> AvatarBrushProperty =
        AvaloniaProperty.Register<ChatListItem, IBrush>(nameof(AvatarBrush), Brushes.SlateGray);

    public static readonly StyledProperty<bool> IsOnlineProperty =
        AvaloniaProperty.Register<ChatListItem, bool>(nameof(IsOnline), false);

    public static readonly StyledProperty<string> NameProperty =
        AvaloniaProperty.Register<ChatListItem, string>(nameof(Name), string.Empty);

    public static readonly StyledProperty<string> LastMessageProperty =
        AvaloniaProperty.Register<ChatListItem, string>(nameof(LastMessage), string.Empty);

    public static readonly StyledProperty<string> TimeTextProperty =
        AvaloniaProperty.Register<ChatListItem, string>(nameof(TimeText), string.Empty);

    public static readonly StyledProperty<int> UnreadCountProperty =
        AvaloniaProperty.Register<ChatListItem, int>(nameof(UnreadCount), 0);

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<ChatListItem, bool>(nameof(IsSelected), false);

    public static readonly StyledProperty<ICommand?> SelectCommandProperty =
        AvaloniaProperty.Register<ChatListItem, ICommand?>(nameof(SelectCommand));

    public static readonly DirectProperty<ChatListItem, bool> HasUnreadProperty =
        AvaloniaProperty.RegisterDirect<ChatListItem, bool>(nameof(HasUnread), o => o.HasUnread);

    private bool _hasUnread;

    public ChatListItem()
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

    public string LastMessage
    {
        get => GetValue(LastMessageProperty);
        set => SetValue(LastMessageProperty, value);
    }

    public string TimeText
    {
        get => GetValue(TimeTextProperty);
        set => SetValue(TimeTextProperty, value);
    }

    public int UnreadCount
    {
        get => GetValue(UnreadCountProperty);
        set => SetValue(UnreadCountProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public ICommand? SelectCommand
    {
        get => GetValue(SelectCommandProperty);
        set => SetValue(SelectCommandProperty, value);
    }

    public bool HasUnread
    {
        get => _hasUnread;
        private set => SetAndRaise(HasUnreadProperty, ref _hasUnread, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == UnreadCountProperty)
        {
            HasUnread = UnreadCount > 0;
        }
    }
}