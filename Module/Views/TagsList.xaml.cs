using System.Windows.Controls;
using VideoPlayer.Infrastructure;

namespace Module
{
    /// <summary>
    ///     Interaction logic for TagsList.xaml
    /// </summary>
    public partial class TagsList : UserControl, ITagsListView
    {
        public TagsList()
        {
            this.InitializeComponent();
        }

        public IViewModel ViewModel
        {
            get { return this.DataContext as IViewModel; }
            set { this.DataContext = value; }
        }
    }
}