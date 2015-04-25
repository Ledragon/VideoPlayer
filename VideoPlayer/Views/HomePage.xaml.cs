using VideoPlayer.Common;
using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : IHomePage
    {
        public HomePage()
        {
            this.InitializeComponent();
            this.DataContext = DependencyFactory.Resolve<IHomePageViewModel>();
        }

        public IViewModel ViewModel { get; set; }
    }
}