using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.SettingsModule
{
    /// <summary>
    ///     Interaction logic for SettingsPageButtonView.xaml
    /// </summary>
    public partial class SettingsPageButtonView : ISettingsPageButtonView
    {
        public SettingsPageButtonView(ISettingsButtonViewModel viewModel)
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