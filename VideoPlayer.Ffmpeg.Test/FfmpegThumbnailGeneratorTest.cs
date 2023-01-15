using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPlayer.Ffmpeg.Test
{
    [TestFixture]
    public class FfmpegThumbnailGeneratorTest
    {
        [Test]
        public void Should_Generate_Thumbnails()
        {
            var video = @"F:\Enfants\Disney\Titan Ae.avi";
            var moqInfoProvider = new Mock<IFfprobeInfoExtractor>();
            moqInfoProvider.Setup(x => x.GetVideoInfo(video))
                .Returns(() => new FfprobeVideoInfo { format = new Format { duration = "5277.510844" } });
            var sut = new FfmpegThumbnailGenerator(moqInfoProvider.Object);
            var count = 24;
            var result = sut.GenerateThumbnails(video, count);
            Assert.AreEqual(count, result.Count);
        }
    }
}
