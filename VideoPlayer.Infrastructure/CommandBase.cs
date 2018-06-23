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

        public virtual Boolean CanExecute(Object parameter)
        {
            return true;
        }

        public void Execute(Object parameter)
        {
            this._method.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}