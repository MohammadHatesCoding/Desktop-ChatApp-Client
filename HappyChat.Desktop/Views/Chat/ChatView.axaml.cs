using Avalonia.Controls;
using Avalonia.Interactivity;
using HappyChat.Desktop.ViewModels.Chat;

namespace HappyChat.Desktop.Views.Chat;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();

        Loaded += ChatView_Loaded;
    }

    private void ChatView_Loaded(object? sender, RoutedEventArgs e)
    {
        if (DataContext is ChatViewModel viewModel)
        {
            viewModel.LoadChatsCommand.Execute(null);
        }
    }
}