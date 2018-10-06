using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.PlaylistManagement
{
    public interface IVideosPageButtonViewModel : IViewModel
    {
        ICommand NavigateCommand { get; }
    }
}