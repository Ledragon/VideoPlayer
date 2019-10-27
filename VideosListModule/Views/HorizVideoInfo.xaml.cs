using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosListModule.Views
{
    /// <summary>
    /// Interaction logic for HorizVideoInfo.xaml
    /// </summary>
    public partial class HorizVideoInfo : UserControl, IVideoInfoView
    {
        public HorizVideoInfo(IVideoInfoViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}
