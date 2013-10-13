using System.Windows.Input;

namespace VideoPlayer
{
    public static class Commands
    {
        public static RoutedCommand StopCommand = new RoutedCommand();
        static Commands ()
        {
            StopCommand.InputGestures.Add(new KeyGesture(Key.X));
        }
    }
}
