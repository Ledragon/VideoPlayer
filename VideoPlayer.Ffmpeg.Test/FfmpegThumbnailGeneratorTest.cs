using Moq;
using NUnit.Framework;
using VideoPlayer.Services;

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
            var moqPathService = new Mock<IPathService>();
            moqPathService.Setup(x => x.GetThumbnailDirectory())
                .Returns(() => @"D:\tmp\thumbnails");
            var sut = new FfmpegThumbnailGenerator(moqInfoProvider.Object, moqPathService.Object);
            var count = 24;
            var result = sut.GenerateThumbnails(video, count);
            Assert.AreEqual(count, result.Count);
        }
        
        [Test]
        public void Should_Generate_Contact_Sheet()
        {
            var video = @"F:\Enfants\Disney\Titan Ae.avi";
            var moqInfoProvider = new Mock<IFfprobeInfoExtractor>();
            moqInfoProvider.Setup(x => x.GetVideoInfo(video))
                .Returns(() => new FfprobeVideoInfo { format = new Format { duration = "5277.510844" } });
            var moqPathService = new Mock<IPathService>();
            moqPathService.Setup(x => x.GetThumbnailDirectory())
                .Returns(() => @"D:\tmp\thumbnails");
            var sut = new FfmpegThumbnailGenerator(moqInfoProvider.Object, moqPathService.Object);
            var count = 24;
            var result = sut.GenerateContactSheet(video, 6,4);
            Assert.IsTrue(File.Exists(result));
        }
    }
}
