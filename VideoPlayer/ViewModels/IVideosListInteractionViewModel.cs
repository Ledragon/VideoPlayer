using System.Windows.Input;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.ViewModels
{
    public interface IVideosListInteractionViewModel : IViewModel
    {
        ICommand AddAllCommand { get; }
        ICommand PlayAllCommand { get; }
    }
}