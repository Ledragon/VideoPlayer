using Microsoft.Practices.Prism.Regions;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosPageModule
{
    /// <summary>
    ///     Interaction logic for VideosPageButtonView.xaml
    /// </summary>
    [ViewSortHint("1")]
    public partial class VideosPageButtonView : IVideosPageButtonView
    {
        public VideosPageButtonView(IVideosPageButtonViewModel viewModel)
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