using System.Windows.Input;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public interface IHomePageViewModel : IViewModel
    {
        ICommand GoToSettingsCommand { get; set; }
        ICommand GoToVideosCommand { get; set; }
        ICommand LoadCommand { get; set; }
        ICommand CleanCommand { get; set; }
    }
}