using System.Windows.Input;

public abstract class CommandBase : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return CanExecuteCmd(parameter);
    }

    public void Execute(object parameter)
    {
        ExecuteCmd(parameter);
    }

    protected virtual bool CanExecuteCmd(object parameter)
    {
        return true;
    }

    protected abstract void ExecuteCmd(object parameter);
}