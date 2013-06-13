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
            this.PlaySelectedVideo();
        }

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.IsMuted = !this._uiMediaElement.IsMuted;
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                this._uiMediaElement.Stop();
                e.Handled = true;
            }
            else if (e.Key == Key.Space)
            {
                if (this._uiMediaElement.CanPause)
                {
                    this._uiMediaElement.Pause();
                }
                else
                {
                    this._uiMediaElement.Play();

                }
                e.Handled = true;
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.Stop();
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.Pause();
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void PlaySelectedVideo()
        {
            Video video = this._uiFilesListBox.SelectedItem as Video;
            this._uiMediaElement.Source = video.FileUri;
            this._uiMediaElement.Play();
        }
    }
}
