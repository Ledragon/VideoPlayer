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
        private Boolean IsFullScreenVideo = false;
        private PlayState State;

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
                this.State = PlayState.Stopped;
                e.Handled = true;
            }
            else if (e.Key == Key.Space)
            {
                if (this._uiMediaElement.CanPause)
                {
                    this._uiMediaElement.Pause();
                    this.State = PlayState.Paused;
                }
                else
                {
                    this._uiMediaElement.Play();
                    this.State = PlayState.Playing;

                }
                e.Handled = true;
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.Stop();
            this.State = PlayState.Stopped;
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.Pause();
            this.State = PlayState.Paused;
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.State == PlayState.Paused)
            {
                this._uiMediaElement.Play();
                this.State = PlayState.Playing;
            }
            else
            {
                this.PlaySelectedVideo();
            }
        }

        private void PlaySelectedVideo()
        {            
            Video video = this._uiFilesListBox.SelectedItem as Video;
            this._uiMediaElement.Source = video.FileUri;
            this._uiMediaElement.Play();
            this.State = PlayState.Playing;
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsFullScreenVideo)
            {
                this._uiVideosListRow.Height = new GridLength(0);
                this._uiVideoInfosRow.Height = new GridLength(100, GridUnitType.Star);
                this._uiVideoPropertiesColumn.Width = new GridLength(0);
                this._uiMediaElementColumn.Width = new GridLength(100, GridUnitType.Star);
            }
        }

        private enum PlayState
        {
            Playing,
            Stopped,
            Paused
        }
    }
}
