using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Command_
{
    public abstract class Command : ICommand
    {
        bool ICommand.CanExecute(object parameter) => CanExecute(parameter);

        void ICommand.Execute(object parameter) => Execute(parameter);

        protected virtual bool CanExecute(object parameter)
        {
            return true;
        }

        protected abstract void Execute(object? parameter = null);

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, e);
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
        private static readonly Func<object, bool> DefaultCanExecute = _ => true;

        public event EventHandler? CanExecuteChanged;
    }
}
