using Avalonia;
using Avalonia.Controls;

namespace HappyChat.Desktop.Controls.Common;


public partial class OnlineIndicator : UserControl
{

    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<OnlineIndicator, double>(
            nameof(Size),
            10);



    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }



    public static readonly StyledProperty<double> BorderThicknessProperty =
        AvaloniaProperty.Register<OnlineIndicator, double>(
            nameof(BorderThickness),
            2);



    public double BorderThickness
    {
        get => GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }



    public double Radius =>
        Size / 2;



    public OnlineIndicator()
    {
        InitializeComponent();
    }

}