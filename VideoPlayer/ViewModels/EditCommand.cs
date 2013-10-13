using System;
using System.ComponentModel;
using System.Windows.Input;

namespace VideoPlayer.ViewModels
{
    public class EditCommand:ICommand
    {
        public void Execute(object parameter)
        {
        }

        public bool CanExecute(object parameter)
        {
            return false;
        }

        public event EventHandler CanExecuteChanged;
    }
}