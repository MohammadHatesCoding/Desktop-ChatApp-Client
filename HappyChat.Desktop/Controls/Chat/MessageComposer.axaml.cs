//using Avalonia.Controls;

//namespace HappyChat.Desktop.Controls.Chat;


//public partial class MessageComposer : UserControl
//{
//    public MessageComposer()
//    {
//        InitializeComponent();
//    }
//}

using Avalonia.Controls;
using Avalonia.Input;
using HappyChat.Desktop.ViewModels.Chat;

namespace HappyChat.Desktop.Controls.Chat;

public partial class MessageComposer : UserControl
{
    public MessageComposer()
    {
        InitializeComponent();
        MessageInput.KeyDown += MessageInput_KeyDown;
    }

    private void MessageInput_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
        {
            return;
        }

        if (DataContext is not ChatViewModel viewModel)
            return;

        if (viewModel.SendMessageCommand.CanExecute(null))
        {
            viewModel.SendMessageCommand.Execute(null);
        }

        e.Handled = true;
    }
}