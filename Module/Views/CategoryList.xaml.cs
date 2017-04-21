using System.Windows.Controls;
using Module.Interfaces;
using VideoPlayer.Infrastructure.ViewFirst;

namespace Module
{
    /// <summary>
    ///     Interaction logic for CategoryList.xaml
    /// </summary>
    public partial class CategoryList : UserControl, ICategoryListView
    {
        public CategoryList(ICategoryListViewModel categoryListViewModel)
        {
            this.InitializeComponent();
            this.ViewModel = categoryListViewModel;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel) this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}