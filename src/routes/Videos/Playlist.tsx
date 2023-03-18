import {
  Box,
  List,
  ListItem,
  ListItemText,
  Paper,
  Typography
} from '@mui/material';
import { useSelector } from 'react-redux';
import { getVideoDictionary, useGetVideosQuery } from '../../redux';
import { RootState } from '../../redux/configureStore';

interface PropType {}

const Playlist = (props: PropType) => {
  const playlistVideos = useSelector(
    (state: RootState) => state.playlist.videos
  );
  const videos = useSelector((state: RootState) => getVideoDictionary(state));
  console.log(videos);
  return (
    <>
      <Typography
        sx={{ p: 2 }}
        variant="body1"
      >
        Playlist
      </Typography>
      <List>
        {playlistVideos.map((d: number) => (
          <ListItem key={d}>
            <ListItemText>{videos[d]?.title}</ListItemText>
          </ListItem>
        ))}
      </List>
    </>
  );
};

export default Playlist;
