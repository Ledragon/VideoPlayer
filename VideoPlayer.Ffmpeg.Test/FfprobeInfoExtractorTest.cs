using NUnit.Framework;

namespace VideoPlayer.Ffmpeg.Test
{
    [TestFixture]
    public class FfprobeInfoExtractorTest
    {
        [Test]
        public void Should_Read_Video_Info()
        {
            var sut = new FfprobeInfoExtractor();
            var result = sut.GetVideoInfo(@"F:\Enfants\Disney\Titan Ae.avi");
            Assert.AreEqual(2, result.streams.Length);
        }
    }
}