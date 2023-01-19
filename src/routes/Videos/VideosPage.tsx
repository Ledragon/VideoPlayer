import { useEffect, useState } from 'react';
import { Box, Paper } from '@mui/material';
import { Video } from '../../models/Video';
import VideoDialog from './VideoDialog';
import VideosDataGrid from './VideosDataGrid';
import { apiUrl } from '../../fetchHelper';
import { useGetVideosQuery } from '../../redux/api';

interface State {
  // videos: Video[];
  selectedVideo: Video | null;
}

const VideosPage = () => {
  const [state, setState] = useState<State>({
    // videos: [],
    selectedVideo: null
  });
  const [open, setOpen] = useState(false);
  const { data: videos, isLoading } = useGetVideosQuery();

  function handleClick(v: Video) {
    setState({ ...state, selectedVideo: v });
    setOpen(true);
  }
  const { selectedVideo } = state;
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
          flexBasis: !!selectedVideo ? '50%' : '100%',
          flexShrink: 1,
          m: 1
        }}
      >
        <VideosDataGrid
          videos={videos||[]}
          onClick={handleClick}
        ></VideosDataGrid>
      </Paper>
      {!!selectedVideo ? (
        <Paper sx={{ flexBasis: '50%', m: 1 }}></Paper>
      ) : (
        <></>
      )}
      {/* <VideosImageList
        videos={state.videos}
        onClick={handleClick}
      ></VideosImageList> */}
      <VideoDialog
        open={open}
        video={selectedVideo}
        onSave={() => setOpen(false)}
        onCancel={() => setOpen(false)}
      ></VideoDialog>
    </Box>
  );
};

export default VideosPage;
