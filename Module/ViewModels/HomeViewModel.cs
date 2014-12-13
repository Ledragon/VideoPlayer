using System.Windows.Input;
using VideoPlayer.Infrastructure;

namespace Module
{
    public class HomeViewModel : ViewModelBase, IHomeViewModel
    {
        public HomeViewModel(IHomeView homeView) : base(homeView)
        {
            this.SettingsCommand = new GenericCommand(this.NavigateToSettings);
        }

        public ICommand SettingsCommand { get; set; }

        private void NavigateToSettings()
        {
        }
    }
}