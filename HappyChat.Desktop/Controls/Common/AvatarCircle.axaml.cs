using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HappyChat.Desktop.Controls.Common;

public partial class AvatarCircle : UserControl
{
    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<AvatarCircle, double>(
            nameof(Size),
            44);

    public static readonly StyledProperty<double> FontSizeProperty =
        AvaloniaProperty.Register<AvatarCircle, double>(
            nameof(FontSize),
            16);

    public static readonly StyledProperty<string> InitialsProperty =
        AvaloniaProperty.Register<AvatarCircle, string>(
            nameof(Initials),
            string.Empty);

    public static readonly StyledProperty<IBrush> AvatarBrushProperty =
        AvaloniaProperty.Register<AvatarCircle, IBrush>(
            nameof(AvatarBrush),
            Brushes.SlateGray);

    public static readonly StyledProperty<bool> IsOnlineProperty =
        AvaloniaProperty.Register<AvatarCircle, bool>(
            nameof(IsOnline),
            false);

    public AvatarCircle()
    {
        InitializeComponent();
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public double FontSize
    {
        get => GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
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
}