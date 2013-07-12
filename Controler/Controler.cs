using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Classes;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Controlers
{
    public class Controler
    {
        private String[] VideoExtensions = { ".3gp", ".avi", ".divx", ".flv", ".mp4", ".mpeg", ".mpg", ".wmv" };


        public void Save(ObjectsWrapper wrapper)
        {
            this.Save(this.GetDefaultFileName(), wrapper);
        }

        public void Save(String filePath, ObjectsWrapper wrapper)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObjectsWrapper));
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(filePath);
                xmlSerializer.Serialize(streamWriter, wrapper);
            }
            catch
            {
                // TODO logger les erreurs
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }
        }

        public ObjectsWrapper GetObjectsFromFile(String filePath)
        {
            ObjectsWrapper wrapper = null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObjectsWrapper));
                if (File.Exists(filePath))
                {
                    StreamReader streamReader = new StreamReader(filePath);
                    wrapper = xmlSerializer.Deserialize(streamReader) as ObjectsWrapper;
                    streamReader.Close();
                }
            }
            catch
            {
                // TODO logger les erreurs
            }
            return wrapper;
        }


        public ObjectsWrapper GetObjectsFromFile()
        {
            return this.GetObjectsFromFile(this.GetDefaultFileName());
        }

        public List<String> GetVideoFiles(String directory)
        {
            List<String> videoFiles = new List<String>();
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
                {
                    if (Array.IndexOf(this.VideoExtensions, fileInfo.Extension.ToLowerInvariant()) != -1)
                    {
                        videoFiles.Add(fileInfo.FullName);
                    }
                }
            }
            catch
            {
                // TODO logger les erreurs
            }
            return videoFiles;
        }

        public List<String> GetVideoFiles(Classes.Directory directory)
        {
            List<String> videoFiles = new List<String>();
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory.DirectoryPath);
                SearchOption searchOption;
                if (directory.IsIncludeSubdirectories)
                {
                    searchOption = SearchOption.AllDirectories;
                }
                else
                {
                    searchOption = SearchOption.TopDirectoryOnly;
                }
                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", searchOption))
                {
                    if (Array.IndexOf(this.VideoExtensions, fileInfo.Extension.ToLowerInvariant()) != -1)
                    {
                        videoFiles.Add(fileInfo.FullName);
                    }
                }
            }
            catch
            {
                // TODO logger les erreurs
            }
            return videoFiles;
        }

        public String GetDefaultFileName()
        {
            String defaultFolder = this.GetDefaultFolder();
            String libraryPath = Path.Combine(defaultFolder, "Library.xml");
            return libraryPath;
        }

        public String GetDefaultFolder()
        {
            String myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            String videoPlayerPath = Path.Combine(myDocumentsPath, "VideoPlayer");
            if (!System.IO.Directory.Exists(videoPlayerPath))
            {
                System.IO.Directory.CreateDirectory(videoPlayerPath);
            }
            return videoPlayerPath;
        }
    }
}