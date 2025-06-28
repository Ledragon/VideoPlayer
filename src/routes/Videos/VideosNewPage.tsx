import { Box, LinearProgress } from '@mui/material';
import { useEffect, useMemo, useState } from 'react';
import { useSelector } from 'react-redux';
import { useSearchParams } from 'react-router-dom';

import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import {
  getTagList,
  getVideoList,
  useAddThumbnailsMutation,
  useGetTagsQuery,
  useGetTagVideosQuery,
  useGetVideosQuery,
  useUpdateVideoMutation
} from '../../redux';
import { RootState } from '../../redux/configureStore';
import VideoDialog from './VideoDialog';
import VideosFilters from './VideosFilters';
import VideosImageList from './VideosImageList';

interface PropType {}

enum SortOrder {
  AlphaAsc,
  AlphaDesc,
  DateAsc,
  DateDesc
}

const VideosNewPage = (props: PropType) => {
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [selectedTags, setSelectedTags] = useState<Tag[]>([]);
  const [filteredVideos, setFilteredVideos] = useState<Video[]>([]);
  const [sortOrder, setSortOrder] = useState<SortOrder>(SortOrder.AlphaAsc);
  const [showMissingThumb, setShowMissingThumb] = useState(false);
  const [showMissingCS, setShowMissingCS] = useState(false);
  const [showContactSheets, setShowContactSheets] = useState(true);
  const [searchParams, setSearchParams] = useSearchParams();

  const { isLoading, isFetching } = useGetVideosQuery();
  const { isLoading: isLoadingTags } = useGetTagsQuery();
  const { data: tagVideos, isFetching: isLoadingTagVideos } = useGetTagVideosQuery();
  const [updateVideo, result] = useUpdateVideoMutation();
  const [addThumbnails] = useAddThumbnailsMutation();

  const videos = useSelector((state: RootState) => getVideoList(state));
  const tags = useSelector((state: RootState) => getTagList(state));

  useEffect(() => {
    if (result?.error) {
      console.error(result.error);
    }
  }, [result.error]);

  useEffect(() => {
    setFilteredVideos(videos || []);
    if (selectedVideo) {
      setSelectedVideo(videos?.find((v) => v.id === selectedVideo.id) || null);
    }
  }, [videos]);

  useEffect(() => {
    const urlId = searchParams.get('video');
    if (urlId && videos) {
      setSelectedVideo(videos.find((v) => v.id === parseInt(urlId)) || null);
    }
  }, [searchParams, videos]);

  const handleVideoClick = (video: Video) => {
    if (video) {
      setSelectedVideo(video);
      setSearchParams({ video: video.id.toString(), tab: '0' });
    }
  };

  const handleTagsChange = (_e: any, values: Tag[]) => {
    setSelectedTags(values);
    if (values.length) {
      const st = values.map((d) => d.id);
      const ftv = tagVideos?.filter((d) => st.indexOf(d.tagId) >= 0).map((d) => d.videoId) || [];
      const fv = videos?.filter((v) => ftv.indexOf(v.id) >= 0);
      setFilteredVideos(fv || []);
    } else {
      setFilteredVideos(videos || []);
    }
  };

  const handleSave = (video: Video, thumbnails: string[]) => {
    Promise.all([updateVideo(video).unwrap(), addThumbnails(thumbnails.map((t) => ({ videoId: video.id, image: t, id: 0 }))).unwrap()]).then(
      () => {}
      // setSelectedVideo(null)
    );
  };

  const handleCancel = () => {
    setSelectedVideo(null);
    setSearchParams({});
  };

  const handleAlphaSort = () => {
    setSortOrder(sortOrder === SortOrder.AlphaAsc ? SortOrder.AlphaDesc : SortOrder.AlphaAsc);
  };

  const handleDateSort = () => {
    setSortOrder(sortOrder === SortOrder.DateDesc ? SortOrder.DateAsc : SortOrder.DateDesc);
  };

  const handleThumbChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setShowMissingThumb(e.target.checked);
  };

  const handleCSChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setShowMissingCS(e.target.checked);
  };

  const displayedVideos = useMemo(() => {
    const toSort = filteredVideos.map((d) => d);
    return toSort.sort((a, b) => {
      switch (sortOrder) {
        case SortOrder.AlphaAsc:
          return a.title > b.title ? 1 : -1;
        case SortOrder.AlphaDesc:
          return a.title < b.title ? 1 : -1;
        case SortOrder.DateAsc:
          return a.dateAdded > b.dateAdded ? 1 : -1;
        case SortOrder.DateDesc:
          return a.dateAdded < b.dateAdded ? 1 : -1;
      }
    });
  }, [sortOrder, filteredVideos, showMissingCS, showMissingThumb]);

  const handlePreviousClick = () => {
    if (selectedVideo) {
      const index = displayedVideos.indexOf(selectedVideo);
      if (index > 0) {
        const video = displayedVideos[index - 1];
        setSelectedVideo(video);
        setSearchParams({ video: video.id.toString() });
      }
    }
  };

  const handleNextClick = () => {
    if (selectedVideo) {
      const index = displayedVideos.indexOf(selectedVideo);
      if (index < displayedVideos.length - 1) {
        const video = displayedVideos[index + 1];
        setSelectedVideo(video);
        setSearchParams({ video: video.id.toString() });
      }
    }
  };

  const handleAddAll = () => {};

  const isLoadingAny = isLoading || isFetching || isLoadingTagVideos || isLoadingTags;
  const handleTitleSearchChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const searchTerm = event.target.value.toLowerCase();
    if (searchTerm) {
      const filtered = filteredVideos.filter((video) => video.title.toLowerCase().includes(searchTerm)) || [];
      setFilteredVideos(filtered);
    } else {
      setFilteredVideos(videos || []);
    }
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        p: 1,
        flexGrow: 1,
        flexShrink: 1,
        height: '100%'
      }}>
      {isLoadingAny ? (
        <Box>
          <LinearProgress></LinearProgress>
        </Box>
      ) : (
        <></>
      )}
      <VideosFilters
        tags={tags}
        tagVideos={tagVideos || []}
        selectedTags={selectedTags}
        displayedVideos={displayedVideos}
        showContactSheets={showContactSheets}
        onTagsChange={handleTagsChange}
        onTitleSearchChange={handleTitleSearchChange}
        onAlphaSort={handleAlphaSort}
        onDateSort={handleDateSort}
        onShowContactSheetsChange={setShowContactSheets}
      />
      <Box
        sx={{
          display: 'flex',
          height: '100%',
          flex: '1 1 auto',
          overflow: 'hidden'
        }}
        id="central-part">
        <Box
          sx={{
            flexBasis: '80%',
            flexGrow: 1,
            flexShrink: 0,
            flexDirection: 'column',
            display: 'flex'
          }}>
          <Box
            sx={{
              overflowY: 'auto',
              flexGrow: 1,
              flexShrink: 1,
              mx: 1
            }}
            id="videos-images-box">
            <VideosImageList showContactSheets={showContactSheets} videos={displayedVideos} onClick={handleVideoClick} />
          </Box>
        </Box>
      </Box>
      {selectedVideo ? (
        <VideoDialog
          open={!!selectedVideo}
          tags={tags || []}
          tagVideos={(tagVideos || []).filter((v) => v.videoId === selectedVideo?.id)}
          video={selectedVideo}
          onSave={handleSave}
          onCancel={handleCancel}
          error={result.isError ? { message: result.error } : undefined}
          onPrevious={handlePreviousClick}
          onNext={handleNextClick}
        />
      ) : (
        <></>
      )}
    </Box>
  );
};

export default VideosNewPage;
