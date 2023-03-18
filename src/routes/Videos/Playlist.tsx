import {
  Box,
  List,
  ListItem,
  ListItemText,
  Paper,
  Typography
} from '@mui/material';
import { useSelector } from 'react-redux';
import { useGetVideosQuery } from '../../redux';
import { RootState } from '../../redux/configureStore';

interface PropType {}

const Playlist = (props: PropType) => {
  const playlistVideos = useSelector(
    (state: RootState) => state.playlist.videos
  );
  const { data: videos } = useGetVideosQuery();
  return (
    <>
      <Typography sx={{ p: 2 }} variant="body1">Playlist</Typography>
      <List>
        {playlistVideos.map((d: number) => (
          <ListItem key={d}>
            <ListItemText>{d}</ListItemText>
          </ListItem>
        ))}
      </List>
    </>
  );
};

export default Playlist;
