using Avalonia.Controls;
using Avalonia.Interactivity;
using HappyChat.Shared.Enum;

namespace HappyChat.Desktop.Controls.Chat;

public partial class DeleteMessageDialog : Window
{
    public DeleteMessageDialog(string chatTitle)
    {
        InitializeComponent();

        DataContext = this;

        AlsoDeleteText = $"Also Delete For {chatTitle}";
    }

    public string AlsoDeleteText { get; }

    private void CancelButton_OnClick(
        object? sender,
        RoutedEventArgs e)
    {
        Close(null);
    }

    private void DeleteButton_OnClick(
        object? sender,
        RoutedEventArgs e)
    {
        var deleteType =
            DeleteForEveryoneCheckBox.IsChecked == true
                ? DeleteType.ForEveryone
                : DeleteType.ForMe;

        Close(deleteType);
    }
}