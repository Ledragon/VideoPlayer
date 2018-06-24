using System;
using System.IO;
using System.Xml.Serialization;
using Classes;
using LeDragon.Log.Standard;

namespace VideoPlayer.Database.Repository
{
    public class FileVideoRepository : IVideoRepository
    {
        public void Save(String filePath, ObjectsWrapper wrapper)
        {
            this.Logger().InfoFormat("Saving file '{0}'.", filePath);
            if (wrapper != null)
            {
                var xmlSerializer = new XmlSerializer(typeof(ObjectsWrapper));
                StreamWriter streamWriter = null;
                try
                {
                    streamWriter = new StreamWriter(filePath);
                    xmlSerializer.Serialize(streamWriter, wrapper);
                    this.Logger().InfoFormat("File '{0}' saved.", filePath);
                }
                catch (Exception e)
                {
                    this.Logger().Error(e.Message);
                    this.Logger().Error(e.Source);
                }
                finally
                {
                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                    }
                }
            }
        }

        public ObjectsWrapper Load(String filePath)
        {
            var wrapper = new ObjectsWrapper();
            try
            {
                this.Logger().InfoFormat("Loading file '{0}'.", filePath);
                var xmlSerializer = new XmlSerializer(typeof(ObjectsWrapper));
                if (File.Exists(filePath))
                {
                    var streamReader = new StreamReader(filePath);
                    wrapper = xmlSerializer.Deserialize(streamReader) as ObjectsWrapper;
                    streamReader.Close();
                    this.Logger().InfoFormat("File '{0}' loaded.", filePath);
                }
            }
            catch (Exception e)
            {
                this.Logger().Error(e);
            }

            return wrapper;
        }
    }
}