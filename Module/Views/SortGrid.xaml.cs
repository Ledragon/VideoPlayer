using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace Module.Views
{
    /// <summary>
    ///     Interaction logic for SortGrid.xaml
    /// </summary>
    public partial class SortGrid : UserControl, ISortGrid
    {
        public SortGrid()
        {
            this.InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
            set { this.DataContext = value; }
        }
    }

    public interface ISortGrid : IView
    {
    }
}