import {
  Box,
  Card,
  CardHeader,
  CardMedia,
  Chip,
  LinearProgress,
  Paper,
  Stack,
  Typography
} from '@mui/material';
import { useState } from 'react';
import { Video } from '../../models/Video';
import {
  useGetVideosQuery,
  useGetThumbnailsQuery,
  useGetTagsQuery,
  useGetTagVideosQuery
} from '../../redux';
import getBase64Url from './getBase64Url';
import TagChips from './TagChips';
import VideosImageList from './VideosImageList';

interface PropType {}
const VideosNewPage = (props: PropType) => {
  const [video, setSelectedVideo] = useState<Video | null>(null);
  const { data: videos, isLoading } = useGetVideosQuery();
  const { data: thumbnails, isLoading: isLoadingThumbnails } =
    useGetThumbnailsQuery();
  const { data: tags, isLoading: isLoadingTags } = useGetTagsQuery();
  const { data: tagVideos, isLoading: isLoadingTagVideos } =
    useGetTagVideosQuery();

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
        />
      </Box>
    </Box>
  );
};

export default VideosNewPage;
