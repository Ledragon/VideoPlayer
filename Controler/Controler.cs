using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Controlers
{
    public class Controler
    {
        private String[] VideoExtensions = { ".avi", ".mpg", ".mpeg", ".wmv" };

        public void Save(String filePath, ObjectsWrapper wrapper)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObjectsWrapper));
            StreamWriter streamWriter = new StreamWriter(Path.Combine(filePath, "Library.xml"));
            xmlSerializer.Serialize(streamWriter, wrapper);
            streamWriter.Close();
        }

        public ObservableCollection<Video> GetVideos()
        {
            ObservableCollection<Video> videoFiles = new ObservableCollection<Video>();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObjectsWrapper));
                String libraryFile = Path.Combine(System.Environment.SpecialFolder.MyDocuments.ToString(), "Library.xml");
                StreamReader streamReader = new StreamReader(libraryFile);
                if (File.Exists(libraryFile))
                {
                    ObjectsWrapper wrapper = xmlSerializer.Deserialize(streamReader) as ObjectsWrapper;
                    streamReader.Close();
                    videoFiles = wrapper.Videos;
                }
            }
            catch
            {
                //TODO logger les erreurs
            }
            return videoFiles;
        }

        public ObservableCollection<Video> GetVideos(List<String> videoFiles)
        {
            ObservableCollection<Video> videos = new ObservableCollection<Video>();
            foreach (String videoFile in videoFiles)
            {
                videos.Add(new Video(videoFile));
            }
            return videos;
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
                //TODO logger les erreurs
            }
            return videoFiles;
        }
    }
}