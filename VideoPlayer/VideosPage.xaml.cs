﻿using Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vlc.DotNet.Core;
using Path = System.IO.Path;
using Vlc.DotNet.Core.Medias;
using System.Windows.Markup;
using System.Xml;
using Vlc.DotNet.Wpf;
using AxAXVLC;
using System.Drawing;
using ToolLib;
using System.Collections.ObjectModel;


namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage : UserControl
    {

        #region Private Members
        private Boolean IsFullScreenVideo = false;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private DateTime mouseLastMouveDateTime = DateTime.Now;
        private Boolean IsPositionChanging = false;
        private Controlers.Controler controler = new Controlers.Controler();
        ObservableCollection<Video> videos;
        private ObservableCollection<Video> _currentPlayList = new ObservableCollection<Video>();

        private Video NowPlaying
        {
            get
            {
                videos = this.DataContext as ObservableCollection<Video>;
                String path = new Uri(this._VLCcontrol.Media.MRL).LocalPath;
                return this.videos.FirstOrDefault(v => v.FileName == path);
            }
        }

        #endregion

        #region Constructors

        public VideosPage()
        {
            // Set libvlc.dll and libvlccore.dll directory path
            String programFilesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = System.IO.Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath, "plugins");

            //// refer to http://wiki.videolan.org/VLC_command-line_help for more information
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            //VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.StartupOptions.AddOption("--no-snapshot-preview");
            VlcContext.StartupOptions.AddOption("--no-mouse-events");
            VlcContext.StartupOptions.AddOption("--no-keyboard-events");
            ////VlcContext.StartupOptions.AddOption("--no-skip-frames");
            ////VlcContext.StartupOptions.AddOption("--directx-hw-yuv");
            ////VlcContext.StartupOptions.AddOption("--directx-3buffering");
            //VlcContext.StartupOptions.AddOption("--ffmpeg-skiploopfilter=4");
            ////VlcContext.StartupOptions.AddOption("--ffmpeg-hw");
            //VlcContext.StartupOptions.AddOption("--vout=directx");


            ////VlcContext.StartupOptions.ShowLoggerConsole = false;
            ////VlcContext.StartupOptions.LogOptions.Verbosity = VlcLogVerbosities.Debug;
            ////VlcContext.StartupOptions.LogOptions.LogInFile = false;
            ////VlcContext.StartupOptions.LogOptions.LogInFilePath = @"D:\videoplayer_vlc.log";
            //// Initialize the VlcContext
            VlcContext.Initialize();

            InitializeComponent();
        }

        #endregion
      
        #region Events

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.PlaySelectedVideo();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                StopVideoPlaying();
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                if (this.IsFullScreenVideo)
                {
                    this.SwitchToWindowMode();
                }
                else
                {
                    this.SwitchToFullScreen();
                }
                e.Handled = true;

            }
            else if (e.Key == Key.Return)
            {
                this.PlaySelectedVideo();
                e.Handled = true;
            }
            else if (e.Key == Key.A)
            {
                this._VLCcontrol.Medias.Add(new PathMedia((this._uiFilesListBox.SelectedItem as Video).FileName));
                this._currentPlayList.Add(this._uiFilesListBox.SelectedItem as Video);
                e.Handled = true;
            }
            else if (e.Key == Key.P)
            {
                this.PlaySelectedVideo(false);
                //this.playlistPosition = 0;
                e.Handled = true;
            }
            else if (e.Key == Key.C)
            {
                if (this._uiPlaylist.IsVisible)
                {
                    this._uiPlaylist.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    this._uiPlaylist.Visibility = System.Windows.Visibility.Visible;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.I)
            {
                //this.ParseAllMedia();
                e.Handled = true;
            }
        }
      
        #region Buttons

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.AudioProperties.IsMute = !this._VLCcontrol.AudioProperties.IsMute;
            if (this._VLCcontrol.AudioProperties.IsMute)
            {
                this._uiFullScreenMuteButton.Content = FindResource("Muted");
            }
            else
            {
                this._uiFullScreenMuteButton.Content = FindResource("Unmuted");
            }
        }

        private void _uiStopButton_Click(object sender, RoutedEventArgs e)
        {
            this.StopVideoPlaying();
        }

        private void _uiPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._VLCcontrol.IsPaused)
            {
                this._VLCcontrol.Play();

            }
            else
            {
                this._VLCcontrol.Play();
            }
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._VLCcontrol.IsPaused)
            {
                this._VLCcontrol.Play();
            }
            else
            {
                this.PlaySelectedVideo();
            }
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToFullScreen();
        }

        private void _uiWindowedButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToWindowMode();
        }

        private void _uiFasterButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Rate *= 2;
            this._uiRate.Text = "Rate: " + this._VLCcontrol.Rate.ToString();
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Rate /= 2;
            this._uiRate.Text = "Rate: " + this._VLCcontrol.Rate.ToString();
        }

        private void _uiFullScreenNextButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Next();
            this.UpdateInfos();
        }

        private void _uiFullScreenPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Previous();
            this.UpdateInfos();
        }

        private void _uiSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.NowPlaying.Title + ".jpg");
            Boolean IsPaused = this._VLCcontrol.IsPaused;
            try
            {
                this._VLCcontrol.Pause();
                System.Threading.Thread.Sleep(100);
                this._VLCcontrol.TakeSnapshot(imgPath, uint.Parse(_VLCcontrol.VideoProperties.Size.Width.ToString("0")), uint.Parse(_VLCcontrol.VideoProperties.Size.Height.ToString("0")));
                this._VLCcontrol.SnapshotTaken += _VLCcontrol_SnapshotTaken;
            }
            catch
            {
                //TODO logger les erreurs
            }
            if (!IsPaused)
            {
                this._VLCcontrol.Play();
            }
        }

        #endregion

        
        #region Slider

        void _uiVLCFullScrenGrid_MouseMove(object sender, MouseEventArgs e)
        {
            this.mouseLastMouveDateTime = DateTime.Now;
            this.Cursor = Cursors.Arrow;
            this._uiFullScreenControlsGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void _uiFullScreenSlider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.PositionChanged -= _VLCcontrol_PositionChanged;
            this.IsPositionChanging = true;
        }

        private void _uiFullScreenSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.Position = (float)this._uiFullScreenSlider.Value;
            this._VLCcontrol.PositionChanged += _VLCcontrol_PositionChanged;
            this.IsPositionChanging = false;
        }

        private void _uiFullScreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsPositionChanging)
            {
                this._VLCcontrol.Position = (float)e.NewValue;
            }
            Int32 seconds = Int32.Parse((this._uiFullScreenSlider.Value * this._VLCcontrol.Duration.TotalSeconds).ToString("0"));
            TimeSpan position = new TimeSpan(0, 0, seconds);
            this._uiPosition.Text = position.ToString("hh\\:mm\\:ss");
        }

        private void _uiFullScreenSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this._VLCcontrol.PositionChanged -= _VLCcontrol_PositionChanged;
            this.IsPositionChanging = true;

            if (e.Delta < 0)
            {
                this._uiFullScreenSlider.Value -= this._uiFullScreenSlider.SmallChange;
            }
            else
            {
                this._uiFullScreenSlider.Value += this._uiFullScreenSlider.SmallChange;
            }
            this._VLCcontrol.PositionChanged += _VLCcontrol_PositionChanged;
            this.IsPositionChanging = false;
        }

        #endregion

        void _VLCcontrol_PositionChanged(VlcControl sender, VlcEventArgs<float> e)
        {
            if (!this.IsPositionChanging)
            {
                this._uiFullScreenSlider.Value = (Double)this._VLCcontrol.Position;
            }
        }
        
        void _VLCcontrol_EndReached(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            this.StopVideoPlaying();
        }

        void _VLCcontrol_SnapshotTaken(VlcControl sender, VlcEventArgs<string> e)
        {
            String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.NowPlaying.Title + ".jpg");
            if (System.IO.File.Exists(imgPath))
            {
                System.Drawing.Image img = new Bitmap(imgPath);
                this.NowPlaying.PreviewImage = img;
                img.Dispose();
                System.IO.File.Delete(imgPath);
            }
            this._VLCcontrol.SnapshotTaken -= _VLCcontrol_SnapshotTaken;
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            this.timer.Interval = 1000;
            this.timer.Tick += timer_Tick;
            this._VLCcontrol.PositionChanged += _VLCcontrol_PositionChanged;
            this._uiPlaylist.DataContext = this._currentPlayList;
        }

        void Media_DurationChanged(MediaBase sender, VlcEventArgs<long> e)
        {
            if (this.NowPlaying.Length.TotalSeconds == 0)
            {
                this.NowPlaying.Length = this._VLCcontrol.Duration;
            }
            this._uiDuration.Text = this.NowPlaying.Length.ToString("hh\\:mm\\:ss");
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - this.mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                this._uiFullScreenControlsGrid.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void _uiVLCFullScrenGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsFullScreenVideo)
            {
                this._uiVLCFullScrenGrid.MouseMove += _uiVLCFullScrenGrid_MouseMove;
            }
            else
            {
                this._uiVLCFullScrenGrid.MouseMove -= _uiVLCFullScrenGrid_MouseMove;
            }
        }

        #endregion

        #region Methods

        private void StopVideoPlaying()
        {
            this._VLCcontrol.Stop();
            this._uiFullScreenSlider.Value = 0;
            this._uiNowPlaying.Text = "Now playing: ";
            this.SwitchToWindowMode();
        }

        private void PlaySelectedVideo(Boolean IsNewPlaylist)
        {
            if (this._VLCcontrol.IsPlaying)
            {
                this._VLCcontrol.Stop();
                this._uiFullScreenSlider.Value = 0;
            }

            Video video = this._uiFilesListBox.SelectedItem as Video;
            if (IsNewPlaylist)
            {
                this._VLCcontrol.Media = new PathMedia(video.FileName);
                this._currentPlayList.Clear();
                this._currentPlayList.Add(video);
                this._VLCcontrol.EndReached += _VLCcontrol_EndReached;
                this._VLCcontrol.Media.DurationChanged += Media_DurationChanged;
            }
            else
            {
            }
            this._VLCcontrol.Play();
            this.UpdateInfos();
            this.SwitchToFullScreen();
        }

        private void PlaySelectedVideo()
        {
            this.PlaySelectedVideo(true);
        }

        private void SwitchToFullScreen()
        {
            this.IsFullScreenVideo = true;
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Visible;
            this.timer.Start();
        }

        private void SwitchToWindowMode()
        {
            this.IsFullScreenVideo = false;
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Hidden;
            //this._uiFilesListBox.Focus();
            this._uiFilesListBox.SelectedItem = this.NowPlaying;
            this.timer.Stop();
            this.Cursor = Cursors.Arrow;
        }

        private void UpdateInfos()
        {
            this._uiDuration.Text = this.NowPlaying.Length.ToString("hh\\:mm\\:ss");
            this._uiNowPlaying.Text = "Now playing: " + this.NowPlaying.Title;
            this._uiPlaylist.SelectedItem = this.NowPlaying;
        }

        private void ParseAllMedia()
        {
            Boolean IsMute = this._VLCcontrol.AudioProperties.IsMute;
            this._VLCcontrol.AudioProperties.IsMute = true;

            foreach (Video video in videos)
            {
                if (video.Length == TimeSpan.Zero)
                {
                    this._VLCcontrol.Media = new PathMedia(video.FileName);
                    this._VLCcontrol.Play();
                    System.Threading.Thread.Sleep(100);
                    video.Length = this._VLCcontrol.Media.Duration;
                    this._VLCcontrol.Stop();
                }
            }
            this._VLCcontrol.AudioProperties.IsMute = IsMute;
        }

        #endregion
    }
}
