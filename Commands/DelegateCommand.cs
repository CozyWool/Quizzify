using System;

namespace Command_
{
    public class DelegateCommand : Command
    {
        private readonly Func<object, bool> _canExecuteMethod;
        private readonly Action<object> _executeMethod;

        public DelegateCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            _canExecuteMethod = canExecuteMethod;
            _executeMethod = executeMethod;
        }

        protected override bool CanExecute(object parameter)
        {
            return _canExecuteMethod(parameter);
        }

        protected override void Execute(object parameter = null)
        {
            _executeMethod(parameter);
        }
    }
}