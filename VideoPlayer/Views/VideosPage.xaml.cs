using Microsoft.Practices.Unity;
using VideoPlayer.Common;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage
    {
        public VideosPage()
        {
            this.InitializeComponent();
            this.DataContext = Locator.Container.Resolve<VideosTabControlViewModel>();
        }
    }
}