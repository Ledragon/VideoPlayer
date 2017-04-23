using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace Module
{
    /// <summary>
    ///     Interaction logic for TagsList.xaml
    /// </summary>
    public partial class TagsList : UserControl, ITagsListView
    {
        public TagsList(ITagsListViewModel tagsListViewModel)
        {
            this.InitializeComponent();
            this.ViewModel = tagsListViewModel;
        }

        public IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
            set { this.DataContext = value; }
        }
    }
}