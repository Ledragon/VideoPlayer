using Classes;
using System.Collections.Generic;
using System.IO;

namespace VideoPlayer.Nfo
{
    public class NfoService : INfoService
    {
        private INfoSerializer _serializer;

        public NfoService(INfoSerializer nfoSerializer)
        {
            this._serializer = nfoSerializer;
        }
        public void CreateNfo(IEnumerable<Video> videos)
        {
            foreach (var video in videos)
            {
                //var thumbName = video.GetThumbPath();
                //if (!File.Exists(thumbName))
                //{
                //    video.PreviewImage.Save(thumbName);
                //}
                var fileName = Path.GetFileNameWithoutExtension(video.FileName);
                var path = Path.Combine(new FileInfo(video.FileName).DirectoryName, fileName + ".nfo");
                var nfo = video.ToNfo();
                this._serializer.Serialize(nfo, path);

            }
        }
    }
}
