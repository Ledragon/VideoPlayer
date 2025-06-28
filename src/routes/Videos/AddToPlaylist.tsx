import PlaylistAddIcon from '@mui/icons-material/PlaylistAdd';
import { IconButton, Snackbar } from '@mui/material';
import { useState } from 'react';
import { useDispatch } from 'react-redux';

import { Video } from '../../models/Video';
import { addVideo } from '../../redux/playlist.slice';

interface PropType {
  hidden?: boolean;
  video: Video | undefined;
}

const AddToPlaylist = (props: PropType) => {
  const { hidden, video } = props;
  const [open, setOpen] = useState(false);
  const dispatch = useDispatch();

  const onAddToPlaylistButtonClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    if (video) {
      dispatch(addVideo(video.id));
      e.preventDefault();
      e.stopPropagation();
      setOpen(true);
    }
  };

  return (
    <>
      <IconButton onClick={(e) => onAddToPlaylistButtonClick(e)} hidden={hidden} title="Add to playlist" color="secondary">
        <PlaylistAddIcon />
      </IconButton>
      <Snackbar
        open={open}
        onClose={() => {
          setOpen(false);
        }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
        autoHideDuration={3000}
        message={video?.title + ' added to playlist'}
      />
    </>
  );
};

export default AddToPlaylist;
