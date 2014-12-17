using System;
using System.Windows.Controls;
using System.Windows.Input;
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
            this._videosTabControlViewModel = Locator.Container.Resolve<VideosTabControlViewModel>();
            this.DataContext = this._videosTabControlViewModel;
        }

        #region Events

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
            }
        }

        #endregion

        #region Methods

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