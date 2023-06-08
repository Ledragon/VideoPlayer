import InfoIcon from '@mui/icons-material/Info';
import PlaylistAddIcon from '@mui/icons-material/PlaylistAdd';
import VisibilityOffOutlinedIcon from '@mui/icons-material/VisibilityOffOutlined';
import {
  Box,
  Chip,
  IconButton,
  ImageListItem,
  ImageListItemBar
} from '@mui/material';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';
import { useMemo, useState } from 'react';
import {
  useGetContactSheetForVideoQuery,
  useGetThumbnailsForVideoQuery
} from '../../redux';

interface PropType {
  video: Video;
  onInfoClick?: (video: Video) => void;
  onAddToPlaylist?: (video: Video) => void;
}

const VideoImageItem = ({ video, onInfoClick, onAddToPlaylist }: PropType) => {
  const { data: thumbs } = useGetThumbnailsForVideoQuery(video.id);
  const [hovered, setHovered] = useState<boolean>(false);
  const { data: contactSheet } = useGetContactSheetForVideoQuery(video.id);

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

  const image = thumbs?.length ? thumbs[0].image : undefined;

  const formattedDate = useMemo(() => {
    const date = new Date(video.dateAdded);
    return `${
      video.numberOfViews
    } views - added on ${date.getFullYear()}-${date.getMonth()}-${date.getDate()}`;
  }, [video]);

  return (
    <ImageListItem
      onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave}
      id={'video-' + video.id}
    >
      <img
        src={image ? getBase64Url(image) : ''}
        style={{
          maxHeight: '250px'
        }}
      ></img>
      <Chip
        label={video.length}
        sx={{ position: 'absolute', right: 0, bottom: 70 }}
      ></Chip>
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
        subtitle={formattedDate}
        actionIcon={
          <Box sx={{display: 'flex', justifyContent: 'center', alignItems: 'center'}}>
            {hovered ? (
              <IconButton
                onClick={onInfoButtonClick}
                hidden={!onInfoClick}
              >
                <InfoIcon />
              </IconButton>
            ) : null}
            {!contactSheet && <VisibilityOffOutlinedIcon fontSize="small" />}
          </Box>
        }
      ></ImageListItemBar>
    </ImageListItem>
  );
};

export default VideoImageItem;
