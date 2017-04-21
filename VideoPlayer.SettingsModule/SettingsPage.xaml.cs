using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.SettingsModule
{
    /// <summary>
    ///     Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl, ISettingsView
    {
        public SettingsPage(ISettingsViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}