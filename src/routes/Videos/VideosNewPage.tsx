import { Box, LinearProgress } from '@mui/material';
import { useState } from 'react';
import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import {
  useGetVideosQuery,
  useGetThumbnailsQuery,
  useGetTagsQuery,
  useGetTagVideosQuery,
  useUpdateVideoMutation
} from '../../redux';
import TagChips from './TagChips';
import VideoDialog from './VideoDialog';
import VideosImageList from './VideosImageList';

interface PropType {}
const VideosNewPage = (props: PropType) => {
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const { data: videos, isLoading } = useGetVideosQuery();
  const { data: thumbnails, isLoading: isLoadingThumbnails } =
    useGetThumbnailsQuery();
  const { data: tags, isLoading: isLoadingTags } = useGetTagsQuery();
  const { data: tagVideos, isLoading: isLoadingTagVideos } =
    useGetTagVideosQuery();
  const [updateVideo, result] = useUpdateVideoMutation();

  const handleVideoClick = (video: Video) => {
    if (video) {
      setSelectedVideo(video);
    }
  };

  const handleSelectChip = (tag: Tag) => {};

  const handleSave = (video: Video) => {
    updateVideo(video)
      .unwrap()
      .then(() => setSelectedVideo(null));
  };

  const handleCancel = () => {
    setSelectedVideo(null);
  };

  const isLoadingAny =
    isLoading || isLoadingTagVideos || isLoadingTags || isLoadingThumbnails;
  return (
    <Box
      sx={{ display: 'flex', flexDirection: 'column', p: 1, height: '100%' }}
    >
      {isLoadingAny ? (
        <Box>
          <LinearProgress></LinearProgress>
        </Box>
      ) : (
        <></>
      )}
      <Box id="tags-chips">
        <TagChips tags={tags || []}></TagChips>
      </Box>
      <Box>
        <VideosImageList
          videos={videos || []}
          thumbnails={thumbnails || []}
          onClick={handleVideoClick}
        />
      </Box>
      {!!selectedVideo ? (
        <VideoDialog
          open={!!selectedVideo}
          tags={tags || []}
          tagVideos={(tagVideos || []).filter(
            (v) => v.videoId === selectedVideo?.id
          )}
          thumbnails={(thumbnails || []).filter(
            (t) => t.videoId === selectedVideo?.id
          )}
          video={selectedVideo}
          onSave={handleSave}
          onCancel={handleCancel}
        />
      ) : (
        <></>
      )}
    </Box>
  );
};

export default VideosNewPage;
