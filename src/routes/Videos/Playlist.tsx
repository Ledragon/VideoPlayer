import {
  Box,
  Button,
  IconButton,
  List,
  ListItem,
  ListItemText
} from '@mui/material';
import DownloadForOfflineOutlinedIcon from '@mui/icons-material/DownloadForOfflineOutlined';
import { useSelector } from 'react-redux';
import { getVideoDictionary } from '../../redux';
import { RootState } from '../../redux/configureStore';
import { useEffect, useState } from 'react';

interface PropType {}

const Playlist = (props: PropType) => {
  const [objectUrl, setObjectUrl] = useState<string>('');
  const playlistVideos = useSelector(
    (state: RootState) => state.playlist.videos
  );
  const videos = useSelector((state: RootState) => getVideoDictionary(state));

  useEffect(() => {
    const content = playlistVideos.map((d) => videos[d]?.fileName).join('\n');
    const blob = new Blob([content], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    setObjectUrl(url);
  }, [playlistVideos]);
  return (
    <>
      <Box>
        <IconButton
          title="Download m3u playlist"
          download="playlist.m3u"
          href={objectUrl}
          disabled={!objectUrl}
        >
          <DownloadForOfflineOutlinedIcon />
        </IconButton>
      </Box>
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
