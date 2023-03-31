import {
  Box,
  Button,
  IconButton,
  List,
  ListItem,
  ListItemText,
  TextField
} from '@mui/material';
import DownloadForOfflineOutlinedIcon from '@mui/icons-material/DownloadForOfflineOutlined';
import DeleteIcon from '@mui/icons-material/Delete';
import { useDispatch, useSelector } from 'react-redux';
import { getVideoDictionary } from '../../redux';
import { RootState } from '../../redux/configureStore';
import { useEffect, useState } from 'react';
import { removeVideo } from '../../redux/playlist.slice';

interface PropType {}

const Playlist = (props: PropType) => {
  const [objectUrl, setObjectUrl] = useState<string>('');
  const [fileName, setFileName] = useState<string>('playlist');
  const dispatch = useDispatch();
  const playlistVideos = useSelector(
    (state: RootState) => state.playlist.videos
  );
  const videos = useSelector((state: RootState) => getVideoDictionary(state));

  const handleRemoveFromPlaylist = (v: number) => {
    dispatch(removeVideo(v));
  };

  const handleFileNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFileName(e.target.value);
  };

  useEffect(() => {
    const content = playlistVideos.map((d) => videos[d]?.fileName).join('\n');
    const blob = new Blob([content], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    setObjectUrl(url);
  }, [playlistVideos]);
  return (
    <>
      <Box sx={{ display: 'flex', p: 1 }}>
        <TextField
          fullWidth
          label="Playlist name"
          name="File name"
          value={fileName}
          onChange={handleFileNameChange}
          size="small"
          margin="dense"
        />
        <IconButton
          title="Download m3u playlist"
          download={fileName + '.m3u'}
          href={objectUrl}
          disabled={!objectUrl}
        >
          <DownloadForOfflineOutlinedIcon />
        </IconButton>
      </Box>
      <List>
        {playlistVideos.map((d: number) => (
          <ListItem
            key={d}
            secondaryAction={
              <IconButton
                edge="end"
                aria-label="delete"
                onClick={() => handleRemoveFromPlaylist(d)}
              >
                <DeleteIcon />
              </IconButton>
            }
          >
            <ListItemText>{videos[d]?.title}</ListItemText>
          </ListItem>
        ))}
      </List>
    </>
  );
};

export default Playlist;
