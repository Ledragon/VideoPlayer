using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for DirectoryBrowser.xaml
    /// </summary>
    public partial class DirectoryBrowser : Window
    {
        public Classes.Directory _directory = new Classes.Directory();

        public DirectoryBrowser()
        {
            InitializeComponent();
        }

        public DirectoryBrowser(Classes.Directory directory)
        {
            InitializeComponent();
            this._directory = directory;
            this._uiNameTextBox.Text = this._directory.DirectoryName;
            this._uiPathText.Text = this._directory.DirectoryPath;
            this._uiSubFoldersCheckBox.IsChecked = this._directory.IsIncludeSubdirectories;
        }

        private void _uiDirectoriesTree_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (String folderName in Directory.GetLogicalDrives())
            {
                try
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = folderName;
                    item.Tag = folderName;
                    item.Items.Add(null);
                    item.Expanded += new RoutedEventHandler(item_Expanded);
                    this._uiDirectoriesTree.Items.Add(item);
                }
                catch (Exception exc)
                {
                    //TODO logger les erreurs
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
                        foreach (String subfolder in Directory.GetDirectories(currentFolder))
                        {
                            DirectoryInfo directoryInfo = new DirectoryInfo(subfolder);
                            FileAttributes fa = directoryInfo.Attributes;
                            string attributes = fa.ToString();
                            //if (!attributes.Contains("Hidden"))
                            //{
                            TreeViewItem newItem = new TreeViewItem();
                            newItem.Header = System.IO.Path.GetFileName(subfolder);
                            newItem.Tag = subfolder;
                            newItem.Items.Add(null);
                            newItem.Expanded += new RoutedEventHandler(item_Expanded);
                            //newItem.Selected += new RoutedEventHandler(Item_Selected);
                            item.Items.Add(newItem);
                            //}
                        }
                    }
                }
                catch (Exception exc)
                {
                    //TODO logger les erreurs
                }
            }
        }

        private void _uiOKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this._directory.DirectoryName = this._uiNameTextBox.Text;
            this._directory.DirectoryPath = this._uiPathText.Text;
            this._directory.IsIncludeSubdirectories = (Boolean)this._uiSubFoldersCheckBox.IsChecked;
            this.Close();
        }
    }
}