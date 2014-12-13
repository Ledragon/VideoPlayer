using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace Module
{
    /// <summary>
    ///     Interaction logic for CategoryList.xaml
    /// </summary>
    public partial class CategoryList : UserControl, ICategoryListView
    {
        public CategoryList()
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