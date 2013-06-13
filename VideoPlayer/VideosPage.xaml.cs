using Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage : UserControl
    {
        public VideosPage()
        {
            InitializeComponent();
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Video video = this._uiFilesListBox.SelectedItem as Video;
            this._uiMediaElement.Source = video.FileUri;
            this._uiMediaElement.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Video video = this._uiFilesListBox.SelectedItem as Video;
            this._uiMediaElement.Source = new Uri(video.FileName);
            //if (this._uiMediaElement.Source != null)
            //{
            //    this._uiMediaElement.Play();
            //}
        }
    }
}
