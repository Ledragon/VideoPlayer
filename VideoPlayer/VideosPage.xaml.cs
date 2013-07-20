using Classes;
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


namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage : UserControl
    {
        private Boolean IsFullScreenVideo = false;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private DateTime mouseLastMouveDateTime = DateTime.Now;
        private Boolean IsPositionChanging = false;
        //private AxVLCPlugin2 _vlcActiveX;
        private Controlers.Controler controler = new Controlers.Controler();
        private Video nowPlaying;
        //VlcControl _VLCcontrol;
        Int32 playlistPosition = 0;

        public VideosPage()
        {
            //// Set libvlc.dll and libvlccore.dll directory path
            //String programFilesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles);
            //VlcContext.LibVlcDllsPath = System.IO.Path.Combine(programFilesPath, @"VideoLan\VLC");

            //// Set the vlc plugins directory path
            //VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath,"plugins");

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
                    this.WindowMode();
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
                this._vlcActiveX.playlist.add("file:///" + (this._uiFilesListBox.SelectedItem as Video).FileName, null, null);
                e.Handled = true;
            }
            else if (e.Key == Key.P)
            {
                this.PlaySelectedVideo(false);
                this.playlistPosition = 0;
                e.Handled = true;
            }
        }

        private void StopVideoPlaying()
        {
            //this._vlcActiveX.playlist.stop();
            this._VLCcontrol.Stop();
            this._uiFullScreenSlider.Value = 0;
            this._uiNowPlaying.Text = "Now playing: ";
            this.WindowMode();
        }

        private void WindowMode()
        {
            this.IsFullScreenVideo = false;
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Hidden;
            this._uiFilesListBox.Focus();
            this.timer.Stop();
            this.Cursor = Cursors.Arrow;
        }

        private void _uiMuteButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlcActiveX.audio.mute = !this._vlcActiveX.audio.mute;
            if (this._vlcActiveX.audio.mute)
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
            // State=4: paused
            if (this._vlcActiveX.input.state == 4)
            {
                this._vlcActiveX.playlist.play();

            }
            else
            {
                this._vlcActiveX.playlist.pause();
            }
        }

        private void _uiPlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._vlcActiveX.input.state == 4)
            {
                this._vlcActiveX.playlist.play();
            }
            else
            {
                this.PlaySelectedVideo();
            }
        }

        private void PlaySelectedVideo(Boolean IsNewPlaylist)
        {
            // State 3: playing
            if (this._vlcActiveX.input.state == 3)
            {
                this._vlcActiveX.playlist.stop();
                this._uiFullScreenSlider.Value = 0;
            }

            Video video = this._uiFilesListBox.SelectedItem as Video;
            if (IsNewPlaylist)
            {
                this._vlcActiveX.playlist.items.clear();
                this._vlcActiveX.playlist.add("file:///" + video.FileName, "", ":no-snapshot-preview");//\":no-overlay\" 
            }
            this._vlcActiveX.Toolbar = false;
            this._uiNowPlaying.Text = "Now playing: " + video.Title;
            this.SwitchToFullScreen();
            this._vlcActiveX.playlist.play();
            this.nowPlaying = video;

            // used for screenshots
            this._VLCcontrol.AudioProperties.IsMute = true;
            this._VLCcontrol.Media = new PathMedia(this.nowPlaying.FileName);
            this._VLCcontrol.Playing += _VLCcontrol_Playing;
        }

        private void PlaySelectedVideo()
        {
            this.PlaySelectedVideo(true);
        }

        private void _VLCcontrol_Playing(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            this._VLCcontrol.Pause();
        }

        private void _uiFullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchToFullScreen();
        }

        private void SwitchToFullScreen()
        {
            this.IsFullScreenVideo = true;
            this._uiVLCFullScrenGrid.Visibility = System.Windows.Visibility.Visible;
            this.timer.Start();
        }

        private void _uiWindowedButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowMode();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
            this.timer.Interval = 1000;
            this.timer.Tick += timer_Tick;
            this._vlcActiveX = new AxVLCPlugin2();
            this._uiVlcHost.Child = this._vlcActiveX;
            this._vlcActiveX.MediaPlayerPositionChanged += _vlcActiveX_MediaPlayerPositionChanged;
            this._vlcActiveX.MediaPlayerPlaying += _vlcActiveX_MediaPlayerPlaying;
            this._vlcActiveX.Toolbar = false;
            this._vlcActiveX.FullscreenEnabled = false;
        }

        void _vlcActiveX_MediaPlayerPlaying(object sender, EventArgs e)
        {
            Video video = this._uiFilesListBox.SelectedItem as Video;
            Int32 seconds = 0;
            Int32 iteration = 0;
            while (seconds == 0 && iteration < 1000)
            {
                seconds = Int32.Parse((this._vlcActiveX.input.Length / 1000).ToString("0"));
                iteration++;
            }
            TimeSpan duration = new TimeSpan(0, 0, seconds);
            video.Length = duration;
            this._uiDuration.Text = video.Length.ToString("hh\\:mm\\:ss");
        }

        void _vlcActiveX_MediaPlayerPositionChanged(object sender, DVLCEvents_MediaPlayerPositionChangedEvent e)
        {
            if (!this.IsPositionChanging)
            {
                this._uiFullScreenSlider.Value = this._vlcActiveX.input.Position;
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - this.mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                this._uiFullScreenControlsGrid.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void _uiFasterButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlcActiveX.input.rate *= 2;
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlcActiveX.input.rate /= 2;
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

        void _uiVLCFullScrenGrid_MouseMove(object sender, MouseEventArgs e)
        {
            this.mouseLastMouveDateTime = DateTime.Now;
            this.Cursor = Cursors.Arrow;
            this._uiFullScreenControlsGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void _uiFullScreenSlider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._vlcActiveX.MediaPlayerPositionChanged -= _vlcActiveX_MediaPlayerPositionChanged;
            this.IsPositionChanging = true;
        }

        private void _uiFullScreenSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._vlcActiveX.input.Position = this._uiFullScreenSlider.Value;
            this._vlcActiveX.MediaPlayerPositionChanged += _vlcActiveX_MediaPlayerPositionChanged;
            this.IsPositionChanging = false;
        }

        private void _uiFullScreenSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.IsPositionChanging)
            {
                this._vlcActiveX.input.Position = e.NewValue;

            }
            Int32 seconds = Int32.Parse((this._uiFullScreenSlider.Value * this._vlcActiveX.input.Length / 1000).ToString("0"));
            TimeSpan position = new TimeSpan(0, 0, seconds);
            this._uiPosition.Text = position.ToString("hh\\:mm\\:ss");
        }

        private void _uiSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.nowPlaying.Title + ".jpg");
            Boolean IsPaused = (this._vlcActiveX.input.state == 4);
            try
            {
                if (this.nowPlaying.Length != TimeSpan.Zero)
                {
                    this._vlcActiveX.playlist.pause();
                    System.Threading.Thread.Sleep(100);
                    this._VLCcontrol.Position = (float)this._vlcActiveX.input.Position;
                    this._VLCcontrol.Play();
                    this._VLCcontrol.TakeSnapshot(imgPath, uint.Parse(_VLCcontrol.VideoProperties.Size.Width.ToString("0")), uint.Parse(_VLCcontrol.VideoProperties.Size.Height.ToString("0")));
                    this._VLCcontrol.SnapshotTaken += _VLCcontrol_SnapshotTaken;
                }
            }
            catch 
            {
                //TODO logger les erreurs
            }
            this._VLCcontrol.Pause();
            if (!IsPaused)
            {
                this._vlcActiveX.playlist.play();
            }
        }

        void _VLCcontrol_SnapshotTaken(VlcControl sender, VlcEventArgs<string> e)
        {
            String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.nowPlaying.Title + ".jpg");
            if (System.IO.File.Exists(imgPath))
            {
                System.Drawing.Image img = new Bitmap(imgPath);
                this.nowPlaying.PreviewImage = img;
                img.Dispose();
                System.IO.File.Delete(imgPath);
            }
            this._VLCcontrol.SnapshotTaken -= _VLCcontrol_SnapshotTaken;
        }

        private void _uiFullScreenSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this._vlcActiveX.MediaPlayerPositionChanged -= _vlcActiveX_MediaPlayerPositionChanged;
            this.IsPositionChanging = true;

            if (e.Delta < 0)
            {
                this._uiFullScreenSlider.Value -= this._uiFullScreenSlider.SmallChange;
            }
            else
            {
                this._uiFullScreenSlider.Value += this._uiFullScreenSlider.SmallChange;
            }
            this._vlcActiveX.MediaPlayerPositionChanged += _vlcActiveX_MediaPlayerPositionChanged;
            this.IsPositionChanging = false;
        }

        private void _uiFullScreenNextButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlcActiveX.playlist.next();
            this.playlistPosition++;
        }

        private void _uiFullScreenPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            this._vlcActiveX.playlist.prev();
        }
    }
}
