﻿using System.Windows.Data;
using Classes;
using System;
using System.ComponentModel;
using System.Linq;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using log4net;
using Vlc.DotNet.Core;
using Path = System.IO.Path;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;
using System.Drawing;
using System.Collections.ObjectModel;
using System.IO;


namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage
    {

        #region Private Members

        private Boolean _isFullScreenVideo;
        private readonly System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        private DateTime _mouseLastMouveDateTime = DateTime.Now;
        private Boolean _isPositionChanging;
        private readonly ObservableCollection<Video> _currentPlayList = new ObservableCollection<Video>();
        private readonly ILog _logger;
        private Boolean _isPlaylist;

        private Video NowPlaying
        {
            get
            {
                ObservableCollection<Video> videos = this.DataContext as ObservableCollection<Video>;
                String path = new Uri(this._VLCcontrol.Media.MRL).LocalPath;
                return videos.FirstOrDefault(v => v.FileName == path);
            }
        }

        #endregion

        #region Constructors

        public VideosPage()
        {
            this._logger = LogManager.GetLogger(typeof (VideosPage));

            // Set libvlc.dll and libvlccore.dll directory path
            String programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            VlcContext.LibVlcDllsPath = Path.Combine(programFilesPath, @"VideoLan\VLC");

            // Set the vlc plugins directory path
            VlcContext.LibVlcPluginsPath = Path.Combine(VlcContext.LibVlcDllsPath, "plugins");

            //// refer to http://wiki.videolan.org/VLC_command-line_help for more information
            VlcContext.StartupOptions.IgnoreConfig = true;
            VlcContext.StartupOptions.AddOption("--no-video-title-show");
            VlcContext.StartupOptions.ScreenSaverEnabled = false;
            VlcContext.StartupOptions.AddOption("--no-snapshot-preview");
            VlcContext.StartupOptions.AddOption("--no-mouse-events");
            VlcContext.StartupOptions.AddOption("--no-keyboard-events");
            VlcContext.StartupOptions.AddOption("--disable-screensaver");
            ////VlcContext.StartupOptions.AddOption("--no-skip-frames");
            //VlcContext.StartupOptions.AddOption("--directx-hw-yuv");
            ////VlcContext.StartupOptions.AddOption("--directx-3buffering");
            //VlcContext.StartupOptions.AddOption("--ffmpeg-skiploopfilter=4");
            //VlcContext.StartupOptions.AddOption("--ffmpeg-hw");
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
            if (!(e.OriginalSource is TextBox))
            {
                if (e.Key == Key.Tab)
                {
                    if (this._isFullScreenVideo)
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
                    this.AddToExistingPlaylist();
                    e.Handled = true;
                }
                else if (e.Key == Key.P)
                {
                    this._isPlaylist = true;
                    this.PlaySelectedVideo(false);
                    //this.playlistPosition = 0;
                    e.Handled = true;
                }
                else if (e.Key == Key.C)
                {
                    if (this._uiPlaylist.IsVisible)
                    {
                        this._uiPlaylist.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this._uiPlaylist.Visibility = Visibility.Visible;
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.Space)
                {
                    if (this._VLCcontrol.IsPaused)
                    {
                        this._VLCcontrol.Play();
                    }
                    else
                    {
                        this._VLCcontrol.Pause();
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.E)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        //ListCollectionView listCollectionView = CollectionViewSource.GetDefaultView(this._uiFilesListBox.Items) as ListCollectionView;
                        //if(listCollectionView!=null)
                        //{
                        //    listCollectionView.Refresh();
                        //}
                        //;
                        this._uiVideoInfoTabControl.SelectedIndex = 0;
                        this._uiFilesListBox.Items.SortDescriptions.Clear();
                        this._uiFilesListBox.Items.SortDescriptions.Add(
                            new SortDescription(this._uiSortComboBox.SelectedItem.ToString(),
                                ListSortDirection.Ascending));
                        this._uiFilesListBox.Items.SortDescriptions.Add(
                            new SortDescription("Title",
                                ListSortDirection.Ascending));
                        
                    }
                    else
                    {
                        this._uiVideoInfoTabControl.SelectedIndex = 1;
                    }
                    e.Handled = true;
                }
                else if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
                {
                    if (this._uiFilterPanel.Visibility == Visibility.Collapsed)
                    {
                        ObservableCollection<Video> videos = this.DataContext as ObservableCollection<Video>;
                        if (videos != null)
                        {
                            var categories =
                                videos.Select(v => v.Category).Distinct().OrderBy(v => v);
                            this._uiCategoryFilterComboBox.ItemsSource = categories;
                        }
                        this._uiFilterPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this._uiFilterPanel.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void AddToExistingPlaylist()
        {
            this.AddToExistingPlaylist(this._uiFilesListBox.SelectedItem as Video);
        }

        private void AddToExistingPlaylist(Video video)
        {
            this._VLCcontrol.Medias.Add(new PathMedia(video.FileName));
            this._currentPlayList.Add(video);
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
                this._VLCcontrol.Pause();
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
        
        private void _uiFasterButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Rate *= 2;
            this._uiRate.Text = "Rate: " + this._VLCcontrol.Rate;
        }

        private void _uiSlowerButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Rate /= 2;
            this._uiRate.Text = "Rate: " + this._VLCcontrol.Rate;
        }

        private void _uiNextButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Next();
            this.UpdateInfos();
        }

        private void _uiPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            this._VLCcontrol.Previous();
            this.UpdateInfos();
        }

        private void _uiSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.NowPlaying.Title + ".jpg");
            Boolean isPaused = this._VLCcontrol.IsPaused;
            try
            {
                this._VLCcontrol.Pause();
                System.Threading.Thread.Sleep(100);
                this._VLCcontrol.TakeSnapshot(imgPath, uint.Parse(_VLCcontrol.VideoProperties.Size.Width.ToString("0")),
                    uint.Parse(_VLCcontrol.VideoProperties.Size.Height.ToString("0")));
                this._VLCcontrol.SnapshotTaken += _VLCcontrol_SnapshotTaken;
            }
            catch (Exception exc)
            {
                this._logger.Error(exc.Message);
                this._logger.Error(exc.Source);
            }
            if (!isPaused)
            {
                this._VLCcontrol.Play();
            }
        }

        #endregion


        #region Slider

        private void _uiVLCFullScrenGrid_MouseMove(object sender, MouseEventArgs e)
        {
            this._mouseLastMouveDateTime = DateTime.Now;
            this.Cursor = Cursors.Arrow;
            this._uiFullScreenControlsGrid.Visibility = Visibility.Visible;
        }

        private void _uiSlider_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.PositionChanged -= _VLCcontrol_PositionChanged;
            this._isPositionChanging = true;
        }

        private void _uiSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this._VLCcontrol.Position = (float) this._uiFullScreenSlider.Value;
            this._VLCcontrol.PositionChanged += _VLCcontrol_PositionChanged;
            this._isPositionChanging = false;
        }

        private void _uiSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this._isPositionChanging)
            {
                this._VLCcontrol.Position = (float) e.NewValue;
            }
            Int32 seconds =
                Int32.Parse((this._uiFullScreenSlider.Value*this._VLCcontrol.Duration.TotalSeconds).ToString("0"));
            TimeSpan position = new TimeSpan(0, 0, seconds);
            this._uiPosition.Text = position.ToString("hh\\:mm\\:ss");
        }

        private void _uiSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this._VLCcontrol.PositionChanged -= _VLCcontrol_PositionChanged;
            this._isPositionChanging = true;

            if (e.Delta < 0)
            {
                this._uiFullScreenSlider.Value -= this._uiFullScreenSlider.SmallChange;
            }
            else
            {
                this._uiFullScreenSlider.Value += this._uiFullScreenSlider.SmallChange;
            }
            this._VLCcontrol.PositionChanged += _VLCcontrol_PositionChanged;
            this._isPositionChanging = false;
        }

        #endregion

        private void _VLCcontrol_PositionChanged(VlcControl sender, VlcEventArgs<float> e)
        {
            if (!this._isPositionChanging)
            {
                this._uiFullScreenSlider.Value = this._VLCcontrol.Position;
            }
        }

        private void _VLCcontrol_EndReached(VlcControl sender, VlcEventArgs<EventArgs> e)
        {
            if (!this._isPlaylist)
            {
                this.StopVideoPlaying();
                this._VLCcontrol.EndReached -= _VLCcontrol_EndReached;
            }
            else
            {
                this._VLCcontrol.Next();
                //this.NowPlaying = this._VLCcontrol.Media
                this.UpdateInfos();
            }

            // TODO gerer la fin d'une video selon qu'on est en playlist ou pas
        }

        private void _VLCcontrol_SnapshotTaken(VlcControl sender, VlcEventArgs<string> e)
        {
            try
            {

                String imgPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), this.NowPlaying.Title + ".jpg");
                if (File.Exists(imgPath))
                {
                    System.Drawing.Image img = new Bitmap(imgPath);
                    this.NowPlaying.PreviewImage = img;
                    img.Dispose();
                    File.Delete(imgPath);
                }
                this._VLCcontrol.SnapshotTaken -= _VLCcontrol_SnapshotTaken;
            }
            catch (Exception ex)
            {
                this._logger.ErrorFormat(ex.Message);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this._uiFilesListBox.Items.SortDescriptions.Add(new SortDescription(this._uiSortComboBox.SelectedItem.ToString(), ListSortDirection.Ascending));
            this._uiFilesListBox.Items.SortDescriptions.Add(
                new SortDescription("Title",
                    ListSortDirection.Ascending));
            this._timer.Interval = 1000;
            this._timer.Tick += timer_Tick;
            this._VLCcontrol.PositionChanged += _VLCcontrol_PositionChanged;
            this._uiPlaylist.DataContext = this._currentPlayList;
        }

        private void Media_DurationChanged(MediaBase sender, VlcEventArgs<long> e)
        {
            this.NowPlaying.Length = this._VLCcontrol.Duration;
            this._uiDuration.Text = this.NowPlaying.Length.ToString("hh\\:mm\\:ss");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - this._mouseLastMouveDateTime > new TimeSpan(0, 0, 2))
            {
                this.Cursor = Cursors.None;
                this._uiFullScreenControlsGrid.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region Methods

        private void StopVideoPlaying()
        {
            this._VLCcontrol.Stop();
            this._uiVLCFullScrenGrid.MouseMove -= _uiVLCFullScrenGrid_MouseMove;
            this._uiFullScreenSlider.Value = 0;
            this._VLCcontrol.Medias.Clear();
            this._uiNowPlaying.Text = "Now playing: ";
            this.SwitchToWindowMode();
        }

        private void PlaySelectedVideo(Boolean isNewPlaylist)
        {
            this._uiVLCFullScrenGrid.MouseMove += _uiVLCFullScrenGrid_MouseMove;
            if (this._VLCcontrol.IsPlaying)
            {
                this._VLCcontrol.Stop();
                this._uiFullScreenSlider.Value = 0;
            }

            Video video = this._uiFilesListBox.SelectedItem as Video;
            if (File.Exists(video.FileName))
            {
                if (isNewPlaylist)
                {
                    this._logger.InfoFormat("Enqued {0}", video.Title);
                    this._VLCcontrol.Media = new PathMedia(video.FileName);
                    this._logger.InfoFormat("Added as media {0}", video.Title);
                    this._currentPlayList.Clear();
                    this._currentPlayList.Add(video);
                    if (this._VLCcontrol.Media.Duration == TimeSpan.Zero)
                    {
                        this._VLCcontrol.Media.DurationChanged += Media_DurationChanged;
                    }
                    this._isPlaylist = false;
                }
                else
                {
                }
                    this._VLCcontrol.EndReached += _VLCcontrol_EndReached;
                this._VLCcontrol.Play();
                this.UpdateInfos();
                this.SwitchToFullScreen();
            }
            else
            {
                MessageBox.Show("File not found");
                this._logger.ErrorFormat("Could not find file {0}", video.FileName);
            }
        }

        private void PlaySelectedVideo()
        {
            this.PlaySelectedVideo(true);
        }

        private void SwitchToFullScreen()
        {
            this._isFullScreenVideo = true;
            this._uiVideosTabControl.SelectedItem = this._uiVideoPlaying;
            this._timer.Start();
        }

        private void SwitchToWindowMode()
        {
            this._isFullScreenVideo = false;
            this._uiVideosTabControl.SelectedItem = this._uiVideosList;
            this._uiFilesListBox.Focus();
            this._uiFilesListBox.SelectedItem = this.NowPlaying;
            this._timer.Stop();
            this.Cursor = Cursors.Arrow;
        }

        private void UpdateInfos()
        {
            //TODO enlever la methode ci-dessous et faire du binding correct
            this._uiDuration.Text = this.NowPlaying.Length.ToString("hh\\:mm\\:ss");
            this._uiNowPlaying.Text = "Now playing: " + this.NowPlaying.Title;
            this._uiPlaylist.SelectedItem = this.NowPlaying;
        }

        #endregion

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this._uiFilesListBox.Visibility == Visibility.Visible)
            {
                this._uiFilesListBox.Focus();
                this._uiFilesListBox.SelectedIndex = 0;
            }
        }
        
        private void _uiFilterButton_Click(object sender, RoutedEventArgs e)
        {
            ICollectionView view = this._uiFilesListBox.Items;
            view.Filter = this.Filter;
        }

        private bool Filter(Object item)
        {
            Boolean result = false;
            try
            {
                Video video = item as Video;
                var isNameOk = video.Title.ToLower().Contains(this._uiFilterTextBox.Text);
                var tagFilter = this._uiTagFilterTextBox.Text.ToLower();
                Boolean isTagOk = false;
                if (!String.IsNullOrEmpty(tagFilter))
                {
                    var filters = tagFilter.Split(new String[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var filter in filters)
                    {
                        isTagOk = isTagOk || video.Tags.Any(t => t.Value.ToLower().Contains(filter));
                    }
                }
                else
                {
                    isTagOk = true;
                }
                var categoryFilter = this._uiCategoryFilterComboBox.SelectedItem.ToString().ToLower();
                Boolean isCategoryOk = false;
                if (!String.IsNullOrEmpty(categoryFilter) && !String.IsNullOrEmpty(video.Category))
                {
                    isCategoryOk = video.Category.ToLower() == categoryFilter;
                }
                else
                {
                    isCategoryOk = true;
                }
                result = isNameOk && isTagOk && isCategoryOk;
            }
            catch (Exception e)
            {
                this._logger.ErrorFormat(e.Message);
            }
            return result;
        }

        private void _uiPlayAll_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in this._uiFilesListBox.Items)
            {
                this.AddToExistingPlaylist(item as Video);
            }
            this.PlaySelectedVideo(false);
        }

        private void _uiVideoPlaying_KeyUp(object sender, KeyEventArgs e)
        {
               if (e.Key == Key.X)
                {
                    StopVideoPlaying();
                    e.Handled = true;
                }

        }

        private void _uiSortComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._uiFilesListBox != null)
            {
                this._uiFilesListBox.Items.SortDescriptions.Clear();
                var sortDescription = this.SortDescription();
                this._uiFilesListBox.Items.SortDescriptions.Add(sortDescription);
            }
        }

        private SortDescription SortDescription()
        {
            SortDescription sortDescription;
            if (this._uiSortComboBox.SelectedItem.ToString() == "Newest")
            {
                sortDescription= new SortDescription("DateAdded", ListSortDirection.Descending);
            }
            else if (this._uiSortComboBox.SelectedItem.ToString() == "Oldest")
            {
                sortDescription = new SortDescription("DateAdded", ListSortDirection.Ascending);
            }
            else
            {
                sortDescription = new SortDescription(this._uiSortComboBox.SelectedItem.ToString(),
                    ListSortDirection.Ascending);
            }
            return sortDescription;
        }

        private void _uiClearFilter_OnClick(object sender, RoutedEventArgs e)
        {
            ICollectionView view = this._uiFilesListBox.Items;
            view.Filter = this.TrueFilter;
        }

        private Boolean TrueFilter(Object item)
        {
            return true;
        }
    }
}
