import {
  IconButton,
  ImageList,
  ImageListItem,
  ImageListItemBar
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';
import { Thumbnail } from '../../models/Thumbnail';

interface PropType {
  thumbnails: Thumbnail[];
  videos: Video[];
  onClick?: (v: Video) => void;
}

const VideosImageList = (props: PropType) => {
  const { thumbnails, videos, onClick } = props;
  return (
    <ImageList
      cols={6}
      gap={6}
      sx={{ m: 1 }}
    >
      {videos.map((v, i) => {
        const currentThumbs =
          thumbnails?.filter((t) => t.videoId === v.id) || [];
        return (
          <ImageListItem key={i}>
            <img
              src={
                currentThumbs.length ? getBase64Url(currentThumbs[0].image) : ''
              }
              style={{ height: '240px' }}
            ></img>
            <ImageListItemBar
              title={v.title}
              subtitle={v.length}
              actionIcon={
                <IconButton
                  onClick={() => {
                    if (onClick) {
                      onClick(v);
                    }
                  }}
                >
                  <InfoIcon />
                </IconButton>
              }
            ></ImageListItemBar>
          </ImageListItem>
        );
      })}
    </ImageList>
  );
};

export default VideosImageList;
