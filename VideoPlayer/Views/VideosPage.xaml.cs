//using Vlc.DotNet.Core;
//using Vlc.DotNet.Core.Medias;
//using Vlc.DotNet.Wpf;

using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Classes;
using Microsoft.Practices.Unity;
using VideoPlayer.Common;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for VideosPage.xaml
    /// </summary>
    public partial class VideosPage
    {
        private readonly VideosTabControlViewModel _videosTabControlViewModel;

        public VideosPage()
        {
            this.InitializeComponent();
            //this.Player.Stopped += this.Player_Stopped;
            this._videosTabControlViewModel = Locator.Container.Resolve<VideosTabControlViewModel>();
            this.DataContext = this._videosTabControlViewModel;
        }

        #region Events

        private void Player_Stopped(object sender, EventArgs e)
        {
            this.SwitchToWindowMode();
        }

        private void _uiVideoPlaying_KeyUp(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.X)
            {
                this.SwitchToWindowMode();
                e.Handled = true;
            }
        }

        private void UiPlayAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.PlayAll();
        }

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
                    e.Handled = true;
                }
                else if (e.Key == Key.A)
                {
                    this.AddToExistingPlaylist();
                    e.Handled = true;
                }
                //else if (e.Key == Key.P)
                //{
                //    this.SwitchToFullScreen();
                //    //this.Player.PlayAll();
                //    e.Handled = true;
                //}
                //else if (e.Key == Key.E)
                //{
                //    if (Keyboard.Modifiers == ModifierKeys.Control)
                //    {
                //        //ICollectionView listCollectionView =
                //        //    CollectionViewSource.GetDefaultView(this._uiFilesListBox.ItemsSource) as ListCollectionView;
                //        //if (listCollectionView != null)
                //        //{
                //        //    listCollectionView.Refresh();
                //        //}
                //        //this._uiVideoInfoTabControl.SelectedIndex = 0;
                //        //this._uiFilesListBox.Items.Refresh();
                //    }
                //    else
                //    {
                //        //this._uiVideoInfoTabControl.SelectedIndex = 1;
                //    }
                //    e.Handled = true;
                //}
            }
        }

        #endregion

        #region Methods

        private void PlayAll()
        {
            //foreach (Object item in this._videosTabControlViewModel.FilteredVideos)
            //{
            //    this.Player.AddVideo((Video) item);
            //}
            //this.Player.PlayAll();
        }

        private void AddToExistingPlaylist()
        {
            //this.Player.AddVideo(this._videosTabControlViewModel.CurrentVideo);
        }

        private void PlayCurrent()
        {
            //Video video = this._videosTabControlViewModel.CurrentVideo;
            //if (video != null && File.Exists(video.FileName))
            //{
            //    this.Player.PlayVideo(video);
            //    this.SwitchToFullScreen();
            //}
            //else
            //{
            //    MessageBox.Show("File not found");
            //}
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