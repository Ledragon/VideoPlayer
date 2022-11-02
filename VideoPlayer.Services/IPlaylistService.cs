﻿using System.Collections.Generic;
using Classes;
using VideoPlayer.Entities;

namespace VideoPlayer.Services
{
    public interface IPlaylistService
    {
        IEnumerable<Video> Playlist { get; set; }
        void Clear();
    }
}