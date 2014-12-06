//using Vlc.DotNet.Core;
//using Vlc.DotNet.Core.Medias;
//using Vlc.DotNet.Wpf;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Classes;
using Log;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage
    {
        private readonly VideosTabControlViewModel _videosTabControlViewModel;

        #region Constructors

        public VideosPage()
        {
            this.InitializeComponent();
            this.Player.Stopped += this.Player_Stopped;
            this._videosTabControlViewModel = new VideosTabControlViewModel();
            this.DataContext = this._videosTabControlViewModel;
        }

        #endregion

        private void Player_Stopped(object sender, EventArgs e)
        {
            this.StopVideoPlaying();
        }
        
        private void _uiFilterButton_Click(Object sender, RoutedEventArgs e)
        {
            ICollectionView view = this._uiFilesListBox.Items;
            view.Filter = this.Filter;
        }

        private Boolean Filter(Object item)
        {
            Boolean result = true;
            try
            {
                var video = item as Video;
                Boolean isNameOk = true;
                Boolean isTagOk = true;
                if (this._uiFilterGrid.Visibility == Visibility.Visible)
                {
                    if (video != null)
                    {
                        isNameOk = video.Title.ToLower().Contains(this._uiFilterTextBox.Text);
                        isTagOk = this.IsTagOk(video);
                    }
                }
                Boolean isCategoryOk = this.IsCategoryOk(video);
                result = isNameOk && isTagOk && isCategoryOk;
            }
            catch (Exception e)
            {
                this.Logger().ErrorFormat(e.Message);
            }
            return result;
        }

        private Boolean IsCategoryOk(Video video)
        {
            string categoryFilter = this._uiCategoryFilterComboBox.SelectedItem.ToString().ToLower();
            Boolean isCategoryOk = true;
            if (!String.IsNullOrEmpty(categoryFilter) && !String.IsNullOrEmpty(video.Category))
            {
                isCategoryOk = video.Category.ToLower() == categoryFilter;
            }
            return isCategoryOk;
        }
        
        private Boolean IsTagOk(Video video)
        {
            Boolean isTagOk = false;
            if (!String.IsNullOrEmpty(this._uiTagFilterTextBox.Text))
            {
                string tagFilter = this._uiTagFilterTextBox.Text.ToLower();
                if (!String.IsNullOrEmpty(tagFilter))
                {
                    string[] filters = tagFilter.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string filter in filters)
                    {
                        isTagOk = isTagOk || video.Tags.Any(t => t.Value.ToLower().Contains(filter));
                    }
                }
                else
                {
                    isTagOk = true;
                }
            }
            else
            {
                isTagOk = true;
            }
            return isTagOk;
        }

        private void _uiPlayAll_OnClick(Object sender, RoutedEventArgs e)
        {
            this.PlayAll();
            //foreach (Object item in this._uiFilesListBox.Items)
            //{
            //    this.Player.AddVideo((Video)item);
            //    //this.AddToExistingPlaylist(item as Video);
            //}
            //this.Player.PlayAll();
            //this.PlaySelectedVideo(false);
        }

        private void PlayAll()
        {
            foreach (Object item in this._videosTabControlViewModel.FilteredVideos)
            {
                this.Player.AddVideo((Video) item);
            }
            this.Player.PlayAll();
        }

        private void _uiVideoPlaying_KeyUp(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                this.StopVideoPlaying();
                e.Handled = true;
            }
        }

        private void _uiClearFilter_OnClick(Object sender, RoutedEventArgs e)
        {
            ICollectionView view = this._uiFilesListBox.Items;
            view.Filter = this.TrueFilter;
        }

        private Boolean TrueFilter(Object item)
        {
            return true;
        }

        private void UiPlayAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.PlayAll();
            //foreach (Object item in this._uiFilesListBox.Items)
            //{
            //    this.AddToExistingPlaylist(item as Video);
            //}
            //this.PlaySelectedVideo(false);
        }

        #region Events

        private void ListBoxItem_MouseDoubleClick(Object sender, MouseButtonEventArgs e)
        {
            this.PlayCurrent();
        }

        private void UserControl_KeyDown(Object sender, KeyEventArgs e)
        {
            if (!(e.OriginalSource is TextBox))
            {
                if (e.Key == Key.Tab)
                {
                    if (this._videosTabControlViewModel.SelectedIndex == 1)
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
                    this.PlayCurrent();
                    //this.SwitchToFullScreen();
                    //this.Player.PlayVideo(this._videosTabControlViewModel.CurrentVideo);
                    e.Handled = true;
                }
                else if (e.Key == Key.A)
                {
                    this.AddToExistingPlaylist();
                    e.Handled = true;
                }
                else if (e.Key == Key.P)
                {
                    this.SwitchToFullScreen();
                    this.Player.PlayAll();
                    //this._isPlaylist = true;
                    //this.PlaySelectedVideo(false);
                    //this.playlistPosition = 0;
                    e.Handled = true;
                }
                    //else if (e.Key == Key.C)
                    //{
                    //    if (this._uiPlaylist.IsVisible)
                    //    {
                    //        this._uiPlaylist.Visibility = Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        this._uiPlaylist.Visibility = Visibility.Visible;
                    //    }
                    //    e.Handled = true;
                    //}
                    //else if (e.Key == Key.Space)
                    //{
                    //    if (this._VLCcontrol.IsPaused)
                    //    {
                    //        this._VLCcontrol.Play();
                    //    }
                    //    else
                    //    {
                    //        this._VLCcontrol.Pause();
                    //    }
                    //    e.Handled = true;
                    //}
                else if (e.Key == Key.E)
                {
                    if (Keyboard.Modifiers == ModifierKeys.Control)
                    {
                        ICollectionView listCollectionView =
                            CollectionViewSource.GetDefaultView(this._uiFilesListBox.ItemsSource) as ListCollectionView;
                        if (listCollectionView != null)
                        {
                            listCollectionView.Refresh();
                        }
                        this._uiVideoInfoTabControl.SelectedIndex = 0;
                        this._uiFilesListBox.Items.Refresh();
                        //this._uiFilesListBox.Items.SortDescriptions.Clear();
                        //this._uiFilesListBox.Items.SortDescriptions.Add(
                        //    new SortDescription(this._uiSortComboBox.SelectedItem.ToString(),
                        //        ListSortDirection.Ascending));
                        //this._uiFilesListBox.Items.SortDescriptions.Add(
                        //    new SortDescription("Title",
                        //        ListSortDirection.Ascending));
                    }
                    else
                    {
                        this._uiVideoInfoTabControl.SelectedIndex = 1;
                    }
                    e.Handled = true;
                }
            }
        }

        private void AddToExistingPlaylist()
        {
            this.Player.AddVideo(this._videosTabControlViewModel.CurrentVideo);
        }
        
        #endregion

        #region Methods

        private void PlayCurrent()
        {
            Video video = this._videosTabControlViewModel.CurrentVideo;
            if (video != null && File.Exists(video.FileName))
            {
                this.Player.PlayVideo(video);
                this.SwitchToFullScreen();
            }
            else
            {
                MessageBox.Show("File not found");
                //this.Logger().ErrorFormat("Could not find file {0}", video.FileName);
            }
        }

        private void StopVideoPlaying()
        {
            //this._uiNowPlaying.Text = "Now playing: ";
            this.SwitchToWindowMode();
        }

        private void SwitchToFullScreen()
        {
            this._videosTabControlViewModel.SwitchToFullScreenCommand.Execute(null);
        }

        private void SwitchToWindowMode()
        {
            this._videosTabControlViewModel.SwitchToWindowCommand.Execute(null);
        }

        #endregion
    }
}