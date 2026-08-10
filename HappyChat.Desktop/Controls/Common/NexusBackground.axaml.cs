using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HappyChat.Desktop.Controls.Common;

public partial class NexusBackground : UserControl
{
    private const double GridSpacing = 32;
    private const double DotRadius = 0.8;

    private readonly IBrush _gridBrush =
        new SolidColorBrush(
            Color.FromArgb(35, 65, 85, 120));

    public NexusBackground()
    {
        InitializeComponent();

        SizeChanged += (_, _) => DrawGrid();
    }

    private void DrawGrid()
    {
        GridCanvas.Children.Clear();

        if (Bounds.Width <= 0 ||
            Bounds.Height <= 0)
        {
            return;
        }

        for (double x = 0;
             x <= Bounds.Width;
             x += GridSpacing)
        {
            for (double y = 0;
                 y <= Bounds.Height;
                 y += GridSpacing)
            {
                var dot = new Border
                {
                    Width = DotRadius * 2,
                    Height = DotRadius * 2,
                    CornerRadius = new CornerRadius(DotRadius),
                    Background = _gridBrush
                };

                Canvas.SetLeft(dot, x - DotRadius);
                Canvas.SetTop(dot, y - DotRadius);

                GridCanvas.Children.Add(dot);
            }
        }
    }
}