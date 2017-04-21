using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace Module
{
    /// <summary>
    ///     Interaction logic for VideoFilterGrid.xaml
    /// </summary>
    public partial class VideoFilterGrid : UserControl, IVideoFilterGrid
    {
        public VideoFilterGrid(IVideoFilterGridViewModel viewModel)
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