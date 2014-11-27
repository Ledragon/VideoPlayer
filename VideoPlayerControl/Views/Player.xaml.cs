using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Log;
using VideoPlayerControl.ViewModels;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;
using Image = System.Drawing.Image;

namespace VideoPlayerControl.Views
{
    /// <summary>
    ///     Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        public delegate void StoppedEventHandler(Object sender, EventArgs e);

        private readonly PlayerViewModel _viewModel;

        public Player()
        {
            this.InitializeVlc();

            this.InitializeComponent();
            this._viewModel = new PlayerViewModel();
            this.DataContext = this._viewModel;

            this._viewModel.PositionChanged += this.ViewModelOnPositionChanged;
            this._viewModel.VideoChanged += this.ViewModelOnVideoChanged;
            this._viewModel.RateChanged += this.ViewModelOnRateChanged;
            this._viewModel.Stopped += this.ViewModelOnStopped;
        }

        private void InitializeVlc()
        {
            String programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath, "plugins");

            // refer to http://wiki.videolan.org/VLC_command-line_help for more information
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.StartupOptions.AddOption("--no-snapshot-preview");
            VlcContext.StartupOptions.AddOption("--no-mouse-events");
            VlcContext.StartupOptions.AddOption("--no-keyboard-events");
            VlcContext.StartupOptions.AddOption("--disable-screensaver");
            // Initialize the VlcContext
            VlcContext.Initialize();
        }

        public void AddVideo(String path)
        {
            this._viewModel.AddVideo(path);
        }

        public event StoppedEventHandler Stopped;

        private void OnStopped(EventArgs e)
        {
            if (this.Stopped != null)
            {
                this.Stopped(this, e);
            }
        }

        #region Buttons click

        private void UiPlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Media = new PathMedia(this._viewModel.CurrentVideo.FileName);
            this._viewModel.IsPaused = false;
        }

        private void UiPauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Pause();
            this._viewModel.IsPaused = true;
        }

        private void UiMuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            this._viewModel.IsMute = !this._viewModel.IsMute;
            this._VLCcontrol.AudioProperties.IsMute = this._viewModel.IsMute;
        }

        private void UiSnapshotButton_OnClick(object sender, RoutedEventArgs e)
        {
            String imgPath = this._viewModel.TemporaryImagePath;
            Boolean isPaused = this._VLCcontrol.IsPaused;
            try
            {
                this._VLCcontrol.Pause();
                Thread.Sleep(100);
                this._VLCcontrol.TakeSnapshot(imgPath,
                    uint.Parse(this._VLCcontrol.VideoProperties.Size.Width.ToString("0")),
                    uint.Parse(this._VLCcontrol.VideoProperties.Size.Height.ToString("0")));
            }
            catch (Exception exc)
            {
                this.Logger().Error(exc.Message);
                this.Logger().Error(exc.Source);
            }
            if (!isPaused)
            {
                this._VLCcontrol.Play();
            }
        }

        #endregion

        #region VLC control events

        private void _VLCcontrol_OnPositionChanged(VlcControl sender, VlcEventArgs<float> e)
        {
            this._viewModel.Position = this._VLCcontrol.Position;
        }

        private void _VLCcontrol_OnSnapshotTaken(VlcControl sender, VlcEventArgs<string> e)
        {
            try
            {
                String imgPath = this._viewModel.TemporaryImagePath;
                if (File.Exists(imgPath))
                {
                    Image img = new Bitmap(imgPath);
                    this._viewModel.CurrentVideo.PreviewImage = img;
                    img.Dispose();
                    File.Delete(imgPath);
                }
            }
            catch (Exception ex)
            {
                this.Logger().ErrorFormat(ex.Message);
            }
        }

        private void _VLCcontrol_OnLengthChanged(VlcControl sender, VlcEventArgs<long> e)
        {
            if (this._viewModel.Duration == TimeSpan.Zero)
            {
                TimeSpan duration = this._VLCcontrol.Duration;
                this._viewModel.Duration = duration;
            }
        }

        #endregion

        #region Slider

        private void UiPositionSlider_PreviewMouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.PositionChanged -= this._VLCcontrol_OnPositionChanged;
        }

        private void UiPositionSlider_PreviewMouseLeftButtonUp(Object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.PositionChanged += this._VLCcontrol_OnPositionChanged;
        }

        #endregion

        #region ViewModel events

        private void ViewModelOnStopped(object sender, EventArgs eventArgs)
        {
            this._VLCcontrol.Stop();
            this.OnStopped(new EventArgs());
        }

        private void ViewModelOnVideoChanged(Object sender, EventArgs eventArgs)
        {
            this._VLCcontrol.Media = new PathMedia(this._viewModel.CurrentVideo.FileName);
        }

        private void ViewModelOnPositionChanged(Object sender, EventArgs eventArgs)
        {
            this._VLCcontrol.Position = this._viewModel.Position;
        }

        private void ViewModelOnRateChanged(Object sender, EventArgs eventArgs)
        {
            this._VLCcontrol.Rate = this._viewModel.Rate;
        }

        #endregion

        private void Player_OnMouseMove(object sender, MouseEventArgs e)
        {
            this._viewModel.MouseMove();
        }
    }
}