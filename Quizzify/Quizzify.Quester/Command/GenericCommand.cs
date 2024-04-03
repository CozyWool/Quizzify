namespace Quizzify.Quester.Command;

public class GenericCommand<T> : CommandBase where T : class
{
    private static readonly Func<T, bool> DefaultCanExecute = _ => true;
    private readonly Action<T> _executeAction;
    private readonly Func<T, bool> _canExecuteFunc;

    public GenericCommand(Action<T> action) : this(action, DefaultCanExecute)
    {
    }

    public GenericCommand(Action<T> executeAction, Func<T, bool> canExecuteAction)
    {
        _executeAction = executeAction;
        _canExecuteFunc = canExecuteAction;
    }

    protected override void ExecuteCmd(object parameter)
    {
        _executeAction(parameter as T);
    }

    protected override bool CanExecuteCmd(object parameter)
    {
        return _canExecuteFunc(parameter as T);
    }
}