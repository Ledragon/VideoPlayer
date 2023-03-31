import InfoIcon from '@mui/icons-material/Info';
import PlaylistAddIcon from '@mui/icons-material/PlaylistAdd';
import {
  Box,
  IconButton,
  ImageListItem,
  ImageListItemBar
} from '@mui/material';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';

interface PropType {
  video: Video;
  image?: string;
  onInfoClick?: (video: Video) => void;
  onAddToPlaylist?: (video: Video) => void;
}

const VideoImageItem = ({
  video,
  image,
  onInfoClick,
  onAddToPlaylist
}: PropType) => {
  const onAddToPlaylistButtonClick = () => {
    if (onAddToPlaylist) {
      onAddToPlaylist(video);
    }
  };
  const onInfoButtonClick = () => {
    if (onInfoClick) {
      onInfoClick(video);
    }
  };
  return (
    <ImageListItem>
      <img
        src={image ? getBase64Url(image) : ''}
        style={{ height: '240px' }}
      ></img>
      <ImageListItemBar
        position="top"
        actionIcon={
          <IconButton
            onClick={onAddToPlaylistButtonClick}
            hidden={!onAddToPlaylist}
          >
            <PlaylistAddIcon />
          </IconButton>
        }
        actionPosition="left"
      ></ImageListItemBar>
      <ImageListItemBar
        title={video.title}
        subtitle={video.length}
        actionIcon={
          <IconButton
            onClick={onInfoButtonClick}
            hidden={!onInfoClick}
          >
            <InfoIcon />
          </IconButton>
        }
      ></ImageListItemBar>
    </ImageListItem>
  );
};

export default VideoImageItem;
