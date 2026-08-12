using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HappyChat.Desktop.Commands;

public sealed class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(
        Action execute,
        Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(
            this,
            EventArgs.Empty);
    }
}

public sealed class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private bool _isExecuting;

    public AsyncRelayCommand(Func<Task> execute)
    {
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return !_isExecuting;
    }

    public async void Execute(object? parameter)
    {
        if (_isExecuting)
            return;

        try
        {
            _isExecuting = true;

            RaiseCanExecuteChanged();

            await _execute();
        }
        finally
        {
            _isExecuting = false;

            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(
            this,
            EventArgs.Empty);
    }
}