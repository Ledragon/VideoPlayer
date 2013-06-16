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
        private GridLength _originalVideosListRow;
        private GridLength _videoInfosRow;
        private GridLength _videoPropertiesColumn;
        private GridLength _mediaElementColumn;
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer(); 

        public VideosPage()
        {
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.IsMuted = !this._uiMediaElement.IsMuted;
            if (this._uiMediaElement.IsMuted)
            {
                this._uiMuteButton.Content = "Unmute";
            }
            else 
            {
                this._uiMuteButton.Content = "Mute"; 
            }
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
            else if (e.Key == Key.Escape && this.IsFullScreenVideo)
            {
                this.SetViewWindowed();
                e.Handled = true;
            }
            else if (e.Key == Key.F)
            {
                this.SetViewFullScreen();
                e.Handled = true; 
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.Stop();
            this._timer.Stop();
            this.State = PlayState.Stopped;
            this._uiSlider.Value = 0;
            this._uiMediaElement.Close();
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.Pause();
            this.State = PlayState.Paused;
            this._timer.Stop();
        
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
            this._timer.Start();
            this.State = PlayState.Playing;
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsFullScreenVideo)
            {
                this.SetViewFullScreen();
            }
            else 
            {
                this.SetViewWindowed();

            }

        }

        private void SetViewFullScreen()
        {
            //Save original settings
            this._originalVideosListRow = new GridLength(this._uiVideosListRow.Height.Value, this._uiVideosListRow.Height.GridUnitType);
            this._videoInfosRow = new GridLength(this._uiVideoInfosRow.Height.Value, this._uiVideoInfosRow.Height.GridUnitType);
            this._videoPropertiesColumn = new GridLength(this._uiVideoPropertiesColumn.Width.Value, this._uiVideoPropertiesColumn.Width.GridUnitType);
            this._mediaElementColumn = new GridLength(this._uiMediaElementColumn.Width.Value, this._uiMediaElementColumn.Width.GridUnitType);

            this._uiVideosListRow.Height = new GridLength(0);
            this._uiVideoInfosRow.Height = new GridLength(100, GridUnitType.Star);
            this._uiVideoPropertiesColumn.Width = new GridLength(0);
            this._uiMediaElementColumn.Width = new GridLength(100, GridUnitType.Star);

            this._uiFullScreenButton.Content = "Windowed";
            this.IsFullScreenVideo = true;
        }

        private void SetViewWindowed()
        {
            this._uiVideosListRow.Height = this._originalVideosListRow;
            this._uiVideoInfosRow.Height = this._videoInfosRow;
            this._uiVideoPropertiesColumn.Width = this._videoPropertiesColumn;
            this._uiMediaElementColumn.Width = this._mediaElementColumn;
            this._uiFullScreenButton.Content = "Full";
            this.IsFullScreenVideo = false;
        }

        private enum PlayState
        {
            Playing,
            Stopped,
            Paused
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (this._uiMediaElement.NaturalDuration.HasTimeSpan)
            {
                this._uiSlider.ValueChanged -= _uiSlider_ValueChanged;
                this._uiSlider.Value = this._uiMediaElement.Position.TotalSeconds;
                this._uiSlider.ValueChanged += _uiSlider_ValueChanged;
            }
        }

        private void _uiMediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (this._uiMediaElement.NaturalDuration.HasTimeSpan)
            {
                this._uiSlider.Maximum = this._uiMediaElement.NaturalDuration.TimeSpan.TotalSeconds;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            this._timer.Interval = 1000;
            this._timer.Tick += _timer_Tick;
            this._uiSlider.ValueChanged += _uiSlider_ValueChanged;
        }

        void _uiSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this._uiMediaElement.Position = TimeSpan.FromSeconds(this._uiSlider.Value);
        }

        private void _uiSlider_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._uiMediaElement.Position = TimeSpan.FromSeconds(this._uiSlider.Value);
        }

        private void _uiFasterButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.SpeedRatio *= 2;
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            this._uiMediaElement.SpeedRatio /= 2;
        }

        private void _uiSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._uiMediaElement.Position = TimeSpan.FromSeconds(this._uiSlider.Value);
        }
    }
}
