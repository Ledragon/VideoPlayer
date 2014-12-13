using VideoPlayer.Infrastructure;

namespace Module
{
    /// <summary>
    ///     Interaction logic for HomePage.xaml
    /// </summary>
    public partial class Home : IHomeView
    {
        public Home()
        {
            this.InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)this.DataContext; }
            set
            {
                this.DataContext = value;
            }
        }
    }
}