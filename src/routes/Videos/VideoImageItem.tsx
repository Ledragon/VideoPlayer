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
import { useState } from 'react';
import { useGetThumbnailsForVideoQuery } from '../../redux';

interface PropType {
  video: Video;
  // image?: string;
  onInfoClick?: (video: Video) => void;
  onAddToPlaylist?: (video: Video) => void;
}

const VideoImageItem = ({
  video,
  // image,
  onInfoClick,
  onAddToPlaylist
}: PropType) => {
  const { data: thumbs } = useGetThumbnailsForVideoQuery(video.id);
  const [hovered, setHovered] = useState<boolean>(false);

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

  const handleMouseEnter = () => {
    setHovered(true);
  };
  const handleMouseLeave = () => {
    setHovered(false);
  };
  const image = thumbs?.length ? thumbs[0].image: undefined;
  return (
    <ImageListItem
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
    >
      <img
        src={image ? getBase64Url(image) : ''}
        style={{
          maxHeight: '200px'
        }}
      ></img>
      {hovered ? (
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
      ) : null}
      <ImageListItemBar
        title={video.title}
        subtitle={video.length}
        actionIcon={
          <>
            {hovered ? (
              <IconButton
                onClick={onInfoButtonClick}
                hidden={!onInfoClick}
              >
                <InfoIcon />
              </IconButton>
            ) : null}
          </>
        }
      ></ImageListItemBar>
    </ImageListItem>
  );
};

export default VideoImageItem;
