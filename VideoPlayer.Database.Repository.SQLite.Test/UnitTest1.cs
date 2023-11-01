namespace VideoPlayer.Database.Repository.SQLite.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            var path = "E:\\Hugues Stefanski\\Documents\\Development\\GitHub\\VideoPlayer\\VideoPlayer\\bin\\Debug\\Files\\LibraryTest.db";
            var context = new VideoPlayerContext(path);
            var rpo = new SqliteVideoRepository(context);
            var result = await rpo.GetVideosWithoutThumbnailsAsync();
            Assert.IsTrue(result.Any());
        }
    }
}