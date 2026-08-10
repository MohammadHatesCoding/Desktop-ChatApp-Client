using Avalonia.Controls;
using HappyChat.Desktop.ViewModels.Auth;

namespace HappyChat.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new CreateAccountViewModel();
    }
}