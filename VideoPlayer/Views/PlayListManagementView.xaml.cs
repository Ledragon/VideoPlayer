using System.Windows.Controls;
using VideoPlayer.Common;
using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;

namespace VideoPlayer.Views
{
    /// <summary>
    ///     Interaction logic for PlayListManagementView.xaml
    /// </summary>
    public partial class PlayListManagementView : UserControl, IPlayListManagementView
    {
        public PlayListManagementView()
        {
            this.InitializeComponent();
            //this.DataContext = DependencyFactory.Resolve<IPlayListManagementViewModel>();
        }


        public IViewModel ViewModel
        {
            get { return (IViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }
    }

    public interface IPlayListManagementView : IView
    {
    }
}