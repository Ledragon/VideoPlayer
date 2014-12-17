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
    }
}