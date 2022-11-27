using System;
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

        public void CreateNfo(IEnumerable<Video> videos, String outDir  = null)
        {
            foreach (var video in videos)
            {
                CreateNfo(video, outDir);
            }
        }

        public void CreateNfo(Video video, String cacheDirectory = null)
        {
            var fileName = Path.GetFileNameWithoutExtension(video.FileName);
            var path = Path.Combine(new FileInfo(video.FileName).DirectoryName, fileName + ".nfo");
            var nfo = video.ToNfo(cacheDirectory);
            this._serializer.Serialize(nfo, path);
        }
    }
}
