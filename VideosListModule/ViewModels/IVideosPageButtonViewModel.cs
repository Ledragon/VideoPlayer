using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosListModule.ViewModels
{
    public interface IVideosPageButtonViewModel : IViewModel
    {
        ICommand NavigateCommand { get; }
    }
}