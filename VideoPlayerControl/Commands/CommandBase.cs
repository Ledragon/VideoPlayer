using System;
using System.Windows.Input;

namespace VideoPlayerControl.Commands
{
    public class CommandBase : ICommand
    {
        private readonly Action _method;

        public CommandBase(Action method)
        {
            this._method = method;
        }

        public virtual Boolean CanExecute(Object parameter)
        {
            return true;
        }

        public virtual void Execute(object parameter)
        {
            this._method.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}