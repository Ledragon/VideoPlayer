using System;

namespace VideoPlayerControl.Commands
{
    public class PreviousCommand : CommandBase
    {
        public PreviousCommand(Action method) : base(method)
        {
        }

        public override bool CanExecute(object parameter)
        {
            Boolean canExecute = base.CanExecute(parameter);
            if (parameter is Boolean)
            {
                canExecute = (Boolean) parameter;
            }
            return canExecute;
        }
    }
}