using System.Collections.Generic;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public interface IRefreshService
    {
        List<Video> Clean(Directory directory);
        List<Video> Load(Directory directory);
    }
}