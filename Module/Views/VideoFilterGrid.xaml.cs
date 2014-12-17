using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace Module
{
    /// <summary>
    ///     Interaction logic for VideoFilterGrid.xaml
    /// </summary>
    public partial class VideoFilterGrid : UserControl, IVideoFilterGrid
    {
        public VideoFilterGrid()
        {
            this.InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}