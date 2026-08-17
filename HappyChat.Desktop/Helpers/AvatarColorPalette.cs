using System;
using Avalonia.Media;

namespace HappyChat.Desktop.Helpers;

public static class AvatarColorPalette
{
    private static readonly string[] Colors =
    {
        "#2563EB",
        "#9333EA",
        "#059669",
        "#DC2626",
        "#EA580C",
        "#0D9488",
        "#DB2777",
        "#4F46E5"
    };

    public static IBrush GetColor(int seed)
    {
        var index = Math.Abs(seed) % Colors.Length;

        return new SolidColorBrush(Color.Parse(Colors[index]));
    }

    public static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "?";

        var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 1)
            return parts[0][..1].ToUpperInvariant();

        return $"{parts[0][..1]}{parts[^1][..1]}".ToUpperInvariant();
    }
}