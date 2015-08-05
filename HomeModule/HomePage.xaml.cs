using VideoPlayer.Infrastructure;

namespace HomeModule
{
    /// <summary>
    ///     Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : IHomePage
    {
        public HomePage()
        {
            this.InitializeComponent();
            //this.DataContext = DependencyFactory.Resolve<IHomePageViewModel>();
        }


        public IViewModel ViewModel
        {
            get { return (IViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}