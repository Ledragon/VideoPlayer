import { useState } from 'react';
import { Backdrop, Box, Button, CircularProgress, Paper } from '@mui/material';
import { Video } from '../../models/Video';
import VideoDialog from './VideoDialog';
import VideosDataGrid from './VideosDataGrid';
import { useGetVideosQuery, useUpdateVideoMutation } from '../../redux/api';
import VideoProperties from './VideoProperties';

const VideosPage = () => {
  const [selectedVideo, setSelectedVideo] = useState<Video | null>(null);
  const [open, setOpen] = useState(false);
  const { data: videos, isLoading } = useGetVideosQuery();
  const [updateVideo, result] = useUpdateVideoMutation();

  function handleClick(v: Video) {
    setSelectedVideo(v);
    setOpen(true);
  }
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
        <Backdrop open={isLoading}>
          <CircularProgress />
        </Backdrop>
        <VideosDataGrid
          videos={videos || []}
          onClick={handleClick}
        ></VideosDataGrid>
      </Paper>
      {!!selectedVideo ? (
        <Paper
          sx={{
            flexBasis: '25%',
            m: 1,
            display: 'flex',
            flexDirection: 'column'
          }}
        >
          {/* <Paper variant="outlined" sx={{ p: 2 }}></Paper> */}
          <Paper variant="outlined" sx={{ p: 2, flexGrow: 1 }}>
            <VideoProperties video={selectedVideo} />
          </Paper>
          <Paper variant="outlined" sx={{ p: 2 }}>
            <Button variant="text">Cancel</Button>
            <Button variant="text" onClick={() => updateVideo(selectedVideo)}>
              Save
            </Button>
          </Paper>
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
