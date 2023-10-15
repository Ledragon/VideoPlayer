import PlaylistAddIcon from '@mui/icons-material/PlaylistAdd';
import { IconButton } from '@mui/material';
import { useDispatch } from 'react-redux';
import { Video } from '../../models/Video';
import { addVideo } from '../../redux/playlist.slice';

interface PropType {
  hidden?: boolean;
  video: Video | undefined;
}

const AddToPlaylist = (props: PropType) => {
  const { hidden, video } = props;
  const dispatch = useDispatch();

  const onAddToPlaylistButtonClick = () => {
    if (!!video) {
      dispatch(addVideo(video.id));
    }
  };

  return (
    <IconButton
      onClick={onAddToPlaylistButtonClick}
      hidden={hidden}
      title="Add to playlist"
      color="secondary"
    >
      <PlaylistAddIcon />
    </IconButton>
  );
};

export default AddToPlaylist;
