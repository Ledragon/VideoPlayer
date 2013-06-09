using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Media;

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
