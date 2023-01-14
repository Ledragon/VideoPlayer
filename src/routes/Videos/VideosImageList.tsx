import {
  IconButton,
  ImageList,
  ImageListItem,
  ImageListItemBar
} from '@mui/material';
import InfoIcon from '@mui/icons-material/Info';
import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';

interface PropType {
  videos: Video[];
  onClick: (v: Video) => void;
}

const VideosImageList = (props: PropType) => {
  const { videos, onClick } = props;
  return (
    <ImageList cols={6} gap={6} sx={{ m: 1.5 }}>
      {videos.map((v, i) => (
        <ImageListItem key={i}>
          <img src={getBase64Url(v.serializedImage)}></img>
          <ImageListItemBar
            title={v.title}
            subtitle={v.length}
            actionIcon={
              <IconButton onClick={() => onClick(v)}>
                <InfoIcon />
              </IconButton>
            }
          ></ImageListItemBar>
        </ImageListItem>
      ))}
    </ImageList>
  );
};

export default VideosImageList;
