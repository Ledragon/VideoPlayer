import { useEffect, useState } from 'react';
import { Box } from '@mui/material';
import { Video } from '../../models/Video';
import VideoDialog from './VideoDialog';
import VideosDataGrid from './VideosDataGrid';
import { apiUrl } from '../../fetchHelper';

interface State {
  videos: Video[];
  selectedVideo: Video | null;
}

const VideosPage = () => {
  const [state, setState] = useState<State>({
    videos: [],
    selectedVideo: null
  });
  const [open, setOpen] = useState(false);
  useEffect(() => {
    fetch(new URL('videos', apiUrl), { mode: 'cors' })
      .then((data) => {
        return data.json();
      })
      .then((data) => {
        setState(Object.assign({}, state, { videos: data }));
      });
  }, []);

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
        overflow: 'hidden'
      }}
    >
      <VideosDataGrid
        videos={state.videos}
        onClick={handleClick}
      ></VideosDataGrid>
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
