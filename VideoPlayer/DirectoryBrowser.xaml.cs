using System;
using System.IO;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for DirectoryBrowser.xaml
    /// </summary>
    public partial class DirectoryBrowser : Window
    {
        public Classes.Directory Directory = new Classes.Directory();

        public DirectoryBrowser()
        {
            InitializeComponent();
        }

        public DirectoryBrowser(Classes.Directory directory)
        {
            InitializeComponent();
            this.Directory = directory;
            this._uiNameTextBox.Text = this.Directory.DirectoryName;
            this._uiPathText.Text = this.Directory.DirectoryPath;
            this._uiSubFoldersCheckBox.IsChecked = this.Directory.IsIncludeSubdirectories;
        }

        private void _uiDirectoriesTree_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (String folderName in System.IO.Directory.GetLogicalDrives())
            {
                try
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = folderName;
                    item.Tag = folderName;
                    item.Items.Add(null);
                    item.Expanded += this.item_Expanded;
                    this._uiDirectoriesTree.Items.Add(item);
                }
                catch (Exception exc)
                {
                    // TODO logger les erreurs
                }
            }
        }

        private void item_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == null)
            {
                try
                {
                    item.Items.Clear();
                    String currentFolder = item.Tag.ToString();
                    DriveInfo driveInfo = new DriveInfo(currentFolder);
                    if (driveInfo.IsReady)
                    {
                        foreach (String subfolder in System.IO.Directory.GetDirectories(currentFolder))
                        {
                            DirectoryInfo directoryInfo = new DirectoryInfo(subfolder);
                            FileAttributes fa = directoryInfo.Attributes;
                            //if (!attributes.Contains("Hidden"))
                            //{
                            TreeViewItem newItem = new TreeViewItem
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
                    // TODO logger les erreurs
                }
            }
        }

        private void _uiOKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Directory.DirectoryName = this._uiNameTextBox.Text;
            this.Directory.DirectoryPath = this._uiPathText.Text;
            this.Directory.IsIncludeSubdirectories = this._uiSubFoldersCheckBox.IsChecked ?? false;
            this.Close();
        }
    }
}