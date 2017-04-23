using System;
using System.Windows.Input;

namespace VideoPlayer.Infrastructure
{
    public abstract class CommandBase : ICommand
    {
        private readonly Action _method;

        public CommandBase(Action method)
        {
            this._method = method;
        }

        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this._method.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}