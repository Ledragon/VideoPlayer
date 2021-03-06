﻿using System.Windows.Controls;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideosListModule.Views
{
    /// <summary>
    /// Interaction logic for HorizVideoMasterDetails.xaml
    /// </summary>
    public partial class HorizVideoMasterDetails : UserControl, IVideosListView
    {
        public HorizVideoMasterDetails(IVideosListViewModel viewModel)
        {
            this.InitializeComponent();
            this.ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get { return (IViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }
    }
}
