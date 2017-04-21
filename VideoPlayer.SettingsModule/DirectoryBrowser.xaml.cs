//using System.Threading.Tasks;

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using log4net;
using Directory = Classes.Directory;
//using System.IO;

namespace VideoPlayer.SettingsModule
{
    /// <summary>
    ///     Interaction logic for DirectoryBrowser.xaml
    /// </summary>
    public partial class DirectoryBrowser : Window
    {
        private readonly ILog _logger;
        public Directory Directory = new Directory();

        public DirectoryBrowser()
        {
            this.InitializeComponent();
        }

        public DirectoryBrowser(Directory directory)
        {
            this._logger = LogManager.GetLogger(typeof (DirectoryBrowser));
            this.Directory = directory;
            this._uiNameTextBox.Text = this.Directory.DirectoryName;
            this._uiPathText.Text = this.Directory.DirectoryPath;
            this._uiSubFoldersCheckBox.IsChecked = this.Directory.IsIncludeSubdirectories;
        }

        private void _uiDirectoriesTree_Loaded(Object sender, RoutedEventArgs e)
        {
            foreach (var folderName in System.IO.Directory.GetLogicalDrives())
            {
                try
                {
                    var item = new TreeViewItem();
                    if (folderName != null)
                    {
                        item.Header = folderName;
                        item.Tag = folderName;
                    }
                    item.Items.Add(null);
                    item.Expanded += this.item_Expanded;
                    this._uiDirectoriesTree.Items.Add(item);
                }
                catch (Exception exc)
                {
                    this._logger.Error(exc.Message);
                    this._logger.Error(exc.Source);
                }
            }
        }

        private void item_Expanded(Object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem) sender;
            if (item.Items.Count == 1 && item.Items[0] == null)
            {
                try
                {
                    item.Items.Clear();
                    var currentFolder = item.Tag.ToString();
                    var driveInfo = new DriveInfo(currentFolder);
                    if (driveInfo.IsReady)
                    {
                        foreach (var subfolder in System.IO.Directory.GetDirectories(currentFolder))
                        {
                            var directoryInfo = new DirectoryInfo(subfolder);
                            var fa = directoryInfo.Attributes;
                            //if (!attributes.Contains("Hidden"))
                            //{
                            var newItem = new TreeViewItem
                            {
                                Header = Path.GetFileName(subfolder),
                                Tag = subfolder
                            };
                            newItem.Items.Add(null);
                            newItem.Expanded += this.item_Expanded;
                            //newItem.Selected += new RoutedEventHandler(Item_Selected);
                            item.Items.Add(newItem);
                            //}
                        }
                    }
                }
                catch (Exception exc)
                {
                    this._logger.Error(exc.Message);
                    this._logger.Error(exc.Source);
                }
            }
        }

        private void _uiOKButton_Click(Object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Directory.DirectoryName = this._uiNameTextBox.Text;
            this.Directory.DirectoryPath = this._uiPathText.Text;
            this.Directory.IsIncludeSubdirectories = this._uiSubFoldersCheckBox.IsChecked ?? false;
            this.Close();
        }
    }
}