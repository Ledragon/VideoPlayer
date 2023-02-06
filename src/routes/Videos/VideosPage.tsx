import { useState } from 'react';
import { Backdrop, Box, Button, CircularProgress, Paper } from '@mui/material';
import { Video } from '../../models/Video';
import VideoDialog from './VideoDialog';
import VideosDataGrid from './VideosDataGrid';
import {
  useAddThumbnailsMutation,
  useGetThumbnailsQuery,
  useGetVideosQuery,
  useUpdateVideoMutation
} from '../../redux';
import VideoProperties from './VideoProperties';
import { Thumbnail } from '../../models/Thumbnail';

const VideosPage = () => {
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [open, setOpen] = useState(false);
  const { data: videos, isLoading } = useGetVideosQuery();
  const { data: thumbnails, isLoading: isLoadingThumbnails } = useGetThumbnailsQuery();
  const [updateVideo, result] = useUpdateVideoMutation();
  const [addThumbnails] = useAddThumbnailsMutation();

  const handleClick = (v: Video) => {
    setSelectedVideo(v);
    setOpen(true);
  };

  const handleSave = (thumbnails: Thumbnail[]) => {
    addThumbnails(thumbnails);
  };
  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'row',
        flexGrow: 1,
        overflow: 'hidden',
        p: 1
      }}
    >
      <Paper
        sx={{
          flexBasis: !!selectedVideo ? '75%' : '100%',
          flexShrink: 1,
          m: 1
        }}
      >
        <Backdrop open={isLoading || isLoadingThumbnails}>
          <CircularProgress />
        </Backdrop>
        <VideosDataGrid
          videos={videos || []}
          thumbnails={thumbnails || []}
          onClick={handleClick}
          onSave = {handleSave}
        ></VideosDataGrid>
      </Paper>
      {!!selectedVideo ? (
        <Paper
          sx={{
            flexBasis: '25%',
            m: 1,
            display: 'flex',
            flexDirection: 'column',
            p: 2
          }}
        >
          <VideoProperties video={selectedVideo} onSubmit={updateVideo} />
        </Paper>
      ) : (
        <></>
      )}
      {/* <VideosImageList
        videos={state.videos}
        onClick={handleClick}
      ></VideosImageList> */}
      {/* <VideoDialog
        open={open}
        video={selectedVideo}
        onSave={() => setOpen(false)}
        onCancel={() => setOpen(false)}
      ></VideoDialog> */}
    </Box>
  );
};

export default VideosPage;
