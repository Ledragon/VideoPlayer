using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Classes
{
    public static class Commands
    {
        private static RoutedCommand _editModeRoutedCommand = new RoutedCommand();

        public static RoutedCommand EditModeRoutedCommand
        {
            get
            {
                return _editModeRoutedCommand;
            }
        }
    }
}
