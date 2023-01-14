import { useEffect, useState } from 'react';
import {
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Paper
} from '@mui/material';
import { Video } from '../../models/Video';
import VideoCard from './VideoCard';
import VideoList from './VideoList';
import VideosImageList from './VideosImageList';
import VideoDialog from './VideoDialog';

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
    fetch('http://localhost:5000/api/videos', { mode: 'cors' })
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
      <VideosImageList
        videos={state.videos}
        onClick={handleClick}
      ></VideosImageList>
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
