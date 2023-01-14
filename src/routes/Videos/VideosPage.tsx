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

interface State {
  videos: Video[];
  selectedVideo: Video | null;
}

const VideosPage = () => {
  const [state, setState] = useState<State>({
    videos: [],
    selectedVideo: null
  });
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
      {/* <Paper sx={{ flexBasis: '50%', overflowY: 'auto' }}>
        <VideoList videos={state.videos} onClick={handleClick}></VideoList>
      </Paper>
      <Paper sx={{ flexBasis: '50%' }}>
        {selectedVideo ? (
          <VideoCard selectedVideo={selectedVideo}></VideoCard>
        ) : (
          <></>
        )}
      </Paper> */}
    </Box>
  );
};

export default VideosPage;
