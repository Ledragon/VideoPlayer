using System.Windows.Input;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public interface IHomePageViewModel : IViewModel
    {
        ICommand GoToSettingsCommand { get; }
        ICommand GoToVideosCommand { get; }
        ICommand LoadCommand { get; }
        ICommand CleanCommand { get; }
    }
}