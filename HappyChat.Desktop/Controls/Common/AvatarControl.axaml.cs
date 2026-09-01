using Avalonia;
using Avalonia.Controls;

namespace HappyChat.Desktop.Controls.Common;


public partial class AvatarControl : UserControl
{

    public static readonly StyledProperty<string> InitialsProperty =
        AvaloniaProperty.Register<AvatarControl, string>(
            nameof(Initials),
            "?");


    public string Initials
    {
        get => GetValue(InitialsProperty);
        set => SetValue(InitialsProperty, value);
    }



    public static readonly StyledProperty<string> AvatarBrushProperty =
        AvaloniaProperty.Register<AvatarControl, string>(
            nameof(AvatarBrush),
            "#2563EB");


    public string AvatarBrush
    {
        get => GetValue(AvatarBrushProperty);
        set => SetValue(AvatarBrushProperty, value);
    }



    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<AvatarControl, double>(
            nameof(Size),
            40);


    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }



    public static readonly StyledProperty<bool> IsOnlineProperty =
        AvaloniaProperty.Register<AvatarControl, bool>(
            nameof(IsOnline),
            false);


    public bool IsOnline
    {
        get => GetValue(IsOnlineProperty);
        set => SetValue(IsOnlineProperty, value);
    }



    public double CornerRadius =>
        Size / 2;



    public double OnlineSize =>
        Size * 0.28;



    public double FontSize =>
        Size * 0.32;



    public AvatarControl()
    {
        InitializeComponent();
    }

}