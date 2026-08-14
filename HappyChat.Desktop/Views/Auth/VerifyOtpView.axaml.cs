using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using System.Linq;

namespace HappyChat.Desktop.Views.Auth;

public partial class VerifyOtpView : UserControl
{
    public VerifyOtpView()
    {
        InitializeComponent();
    }

    private void OtpTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not TextBox currentBox)
            return;

        // فقط اجازه یک رقم
        if (currentBox.Text is not null &&
            currentBox.Text.Length > 1)
        {
            currentBox.Text =
                currentBox.Text[^1..];

            currentBox.CaretIndex =
                currentBox.Text.Length;
        }

        // اگر عدد وارد شد → برو به بعدی
        if (!string.IsNullOrEmpty(currentBox.Text))
        {
            MoveToNext(currentBox);
        }
    }

    private void MoveToNext(TextBox currentBox)
    {
        var boxes = this
            .GetVisualDescendants().OfType<TextBox>().Where(x => x.Classes.Contains("OtpInput")).ToList();

        int index = boxes.IndexOf(currentBox);

        if (index >= 0 &&
            index < boxes.Count - 1)
        {
            boxes[index + 1].Focus();
        }
    }

    private void OtpKeyDown(object? sender, KeyEventArgs e)
    {
        if (sender is not TextBox currentBox)
            return;

        // Backspace روی فیلد خالی → برگشت به قبلی
        if (e.Key == Key.Back &&
            string.IsNullOrEmpty(currentBox.Text))
        {
            var boxes = this
                .GetVisualDescendants()
                .OfType<TextBox>()
                .Where(x => x.Classes.Contains("OtpInput"))
                .ToList();

            int index = boxes.IndexOf(currentBox);

            if (index > 0)
            {
                boxes[index - 1].Focus();
            }
        }
    }
}