using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosPageModule
{
    public interface IVideosPageButtonViewModel : IViewModel
    {
        ICommand NavigateCommand { get; }
    }
}