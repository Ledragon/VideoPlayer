using System.Windows.Input;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.ViewModels
{
    public interface IVideosListInteractionViewModel : IViewModel
    {
        ICommand AddAllCommand { get; }
        ICommand PlayAllCommand { get; }
    }
}