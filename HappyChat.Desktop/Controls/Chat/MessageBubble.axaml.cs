using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HappyChat.Desktop.Controls.Chat;

public partial class MessageBubble : UserControl
{
    public static readonly StyledProperty<string> ContentProperty =
        AvaloniaProperty.Register<MessageBubble, string>(nameof(Content), string.Empty);

    public static readonly StyledProperty<string> TimeTextProperty =
        AvaloniaProperty.Register<MessageBubble, string>(nameof(TimeText), string.Empty);

    public static readonly StyledProperty<bool> IsMineProperty =
        AvaloniaProperty.Register<MessageBubble, bool>(nameof(IsMine), false);

    public static readonly StyledProperty<bool> IsReadProperty =
        AvaloniaProperty.Register<MessageBubble, bool>(nameof(IsRead), false);

    public static readonly StyledProperty<string?> ReactionProperty =
        AvaloniaProperty.Register<MessageBubble, string?>(nameof(Reaction));

    public static readonly StyledProperty<string> SenderInitialsProperty =
        AvaloniaProperty.Register<MessageBubble, string>(nameof(SenderInitials), string.Empty);

    public static readonly DirectProperty<MessageBubble, bool> IsTheirsProperty =
        AvaloniaProperty.RegisterDirect<MessageBubble, bool>(nameof(IsTheirs), o => o.IsTheirs);

    public static readonly DirectProperty<MessageBubble, bool> HasReactionProperty =
        AvaloniaProperty.RegisterDirect<MessageBubble, bool>(nameof(HasReaction), o => o.HasReaction);

    public static readonly DirectProperty<MessageBubble, IBrush> ReadTickBrushProperty =
        AvaloniaProperty.RegisterDirect<MessageBubble, IBrush>(nameof(ReadTickBrush), o => o.ReadTickBrush);

    private bool _isTheirs = true;
    private bool _hasReaction;
    private IBrush _readTickBrush = Brushes.Gray;

    public MessageBubble()
    {
        InitializeComponent();
    }

    public string Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public string TimeText
    {
        get => GetValue(TimeTextProperty);
        set => SetValue(TimeTextProperty, value);
    }

    public bool IsMine
    {
        get => GetValue(IsMineProperty);
        set => SetValue(IsMineProperty, value);
    }

    public bool IsRead
    {
        get => GetValue(IsReadProperty);
        set => SetValue(IsReadProperty, value);
    }

    public string? Reaction
    {
        get => GetValue(ReactionProperty);
        set => SetValue(ReactionProperty, value);
    }

    public string SenderInitials
    {
        get => GetValue(SenderInitialsProperty);
        set => SetValue(SenderInitialsProperty, value);
    }

    public bool IsTheirs
    {
        get => _isTheirs;
        private set => SetAndRaise(IsTheirsProperty, ref _isTheirs, value);
    }

    public bool HasReaction
    {
        get => _hasReaction;
        private set => SetAndRaise(HasReactionProperty, ref _hasReaction, value);
    }

    public IBrush ReadTickBrush
    {
        get => _readTickBrush;
        private set => SetAndRaise(ReadTickBrushProperty, ref _readTickBrush, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsMineProperty)
        {
            IsTheirs = !IsMine;
        }

        if (change.Property == ReactionProperty)
        {
            HasReaction = !string.IsNullOrEmpty(Reaction);
        }

        if (change.Property == IsReadProperty)
        {
            ReadTickBrush = IsRead
                ? new SolidColorBrush(Color.Parse("#2563EB"))
                : new SolidColorBrush(Color.Parse("#7C8BA8"));
        }
    }
}