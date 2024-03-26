import PlaylistAddIcon from '@mui/icons-material/PlaylistAdd';
import { IconButton } from '@mui/material';
import { useDispatch } from 'react-redux';
import { Video } from '../../models/Video';
import { addVideos } from '../../redux/playlist.slice';

interface PropType {
  hidden?: boolean;
  videos: Video[] | undefined;
}

const AddAllToPlaylist = (props: PropType) => {
  const { hidden, videos } = props;
  const dispatch = useDispatch();

  const onAddToPlaylistButtonClick = () => {
    if (!!videos?.length) {
      dispatch(addVideos(videos.map(d=>d.id)));
    }
  };

  return (
    <IconButton
      onClick={onAddToPlaylistButtonClick}
      hidden={hidden}
      title="Add all to playlist"
    >
      <PlaylistAddIcon />
    </IconButton>
  );
};

export default AddAllToPlaylist;
