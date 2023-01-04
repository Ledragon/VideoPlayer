import { useEffect, useState } from 'react';
import {
  Box,
  Card,
  CardHeader,
  CardMedia,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Paper,
} from '@mui/material';
import { Video } from '../Video';


interface State {
  videos: Video[];
  selectedVideo: Video | null;
}

const VideoList = () => {
  const [state, setState] = useState<State>({
    videos: [],
    selectedVideo: null,
  });
  useEffect(() => {
    fetch('http://localhost:5165/api/videos', { mode: 'cors' })
      .then((data) => {
        return data.json();
      })
      .then((data) => {
        console.log(data);
        setState(Object.assign({}, state, { videos: data }));
      });
  }, []);

  function handleClick(v: Video) {
    setState({ ...state, selectedVideo: v });
  }
  const { selectedVideo } = state;
  return (
    <Box sx={{ display: 'flex', flexDirection: 'row', flexGrow: 1 }}>
      <Paper sx={{ flexBasis: '50%' }}>
        <List>
          {state.videos.map((v, i) => {
            return (
              <ListItem key={i} disablePadding>
                <ListItemButton
                  onClick={() => {
                    handleClick(v);
                  }}
                >
                  <ListItemText>{v.title}</ListItemText>
                </ListItemButton>
              </ListItem>
            );
          })}
        </List>
      </Paper>
      <Paper sx={{ flexBasis: '50%' }}>
        {selectedVideo ? (
          <Card>
            <CardHeader title={selectedVideo?.title}></CardHeader>
            <CardMedia
              component="img"
              height={540}
              src={`data:image/png;base64, ${selectedVideo.serializedImage}`}
              alt={selectedVideo.title}
            ></CardMedia>
          </Card>
        ) : (
          <></>
        )}
      </Paper>
    </Box>
  );
};

export default VideoList;
