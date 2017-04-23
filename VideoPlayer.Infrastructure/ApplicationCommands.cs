using Microsoft.Practices.Prism.Commands;

namespace VideoPlayer.Infrastructure
{
    public class ApplicationCommands
    {
        public static CompositeCommand NavigateCommand = new CompositeCommand();
    }
}