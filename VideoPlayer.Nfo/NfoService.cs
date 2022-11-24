using System.Collections.Generic;
using System.IO;
using VideoPlayer.Entities;

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
                var fileName = Path.GetFileNameWithoutExtension(video.FileName);
                var path = Path.Combine(new FileInfo(video.FileName).DirectoryName, fileName + ".nfo");
                var nfo = video.ToNfo();
                this._serializer.Serialize(nfo, path);
            }
        }
    }
}
