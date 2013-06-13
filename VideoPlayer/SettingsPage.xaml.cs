﻿using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;

namespace VideoPlayer
{

    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        public ObservableCollection<Classes.Directory> Directories { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void _uiAddDirectoryButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryBrowser directoryBrowser = new DirectoryBrowser();
            if ((Boolean)directoryBrowser.ShowDialog())
            {
                this.Directories.Add(directoryBrowser._directory);
            }
        }
    }
}
