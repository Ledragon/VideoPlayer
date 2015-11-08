using VideoPlayer.Views;

namespace VideoPlayer.ViewModels
{
    public class PlayListManagementViewModel : Infrastructure.ViewModelBase, IPlayListManagementViewModel
    {
        public PlayListManagementViewModel(IPlayListManagementView view) : base(view)
        {
        }
    }
}