import {
  Avatar,
  Box,
  IconButton,
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  TextField
} from '@mui/material';
import DownloadForOfflineOutlinedIcon from '@mui/icons-material/DownloadForOfflineOutlined';
import DeleteIcon from '@mui/icons-material/Delete';
import { useDispatch, useSelector } from 'react-redux';
import {
  getVideoDictionary,
  useGetContactSheetForVideoQuery
} from '../../redux';
import { RootState } from '../../redux/configureStore';
import { useEffect, useState } from 'react';
import { removeVideo } from '../../redux/playlist.slice';
import { Video } from '../../models/Video';
import { skipToken } from '@reduxjs/toolkit/dist/query';
import getBase64Url from './getBase64Url';
import { timeFormat } from 'd3-time-format';

const tf = timeFormat('%Y-%m-%d');

interface PropType {}

const PlayListItem = ({
  video,
  handleRemoveFromPlaylist
}: {
  video?: Video;
  handleRemoveFromPlaylist: () => void;
}) => {
  const { data: contactSheet } = useGetContactSheetForVideoQuery(
    video?.id || skipToken
  );
  const src = contactSheet ? getBase64Url(contactSheet.image) : undefined;
  return (
    <ListItem
      title={video?.title}
      secondaryAction={
        <IconButton
          edge="end"
          aria-label="delete"
          onClick={() => handleRemoveFromPlaylist()}
          size="small"
        >
          <DeleteIcon fontSize="small" />
        </IconButton>
      }
    >
      <ListItemAvatar sx={{ pr: 2 }}>
        {src ? (
          <img
            src={src}
            width={'60px'}
          />
        ) : (
          <Avatar
            variant="square"
            alt={video?.title}
            src={src}
          />
        )}
      </ListItemAvatar>
      <ListItemText
        primaryTypographyProps={{
          variant: 'caption',
          noWrap: true
        }}
      >
        {video?.title}
      </ListItemText>
    </ListItem>
  );
};

const Playlist = (props: PropType) => {
  const [objectUrl, setObjectUrl] = useState<string>('');
  const date = new Date();
  const [fileName, setFileName] = useState<string>(`${tf(date)}`);
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
      <List
        dense
        sx={{ overflowY: 'auto' }}
      >
        {playlistVideos.map(
          (d: number) =>
            videos[d] && (
              <PlayListItem
                handleRemoveFromPlaylist={() => handleRemoveFromPlaylist(d)}
                video={videos[d]}
              />
            )
        )}
      </List>
    </>
  );
};

export default Playlist;
