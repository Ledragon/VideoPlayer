import {
  ImageList,
  ImageListItem,
  ImageListItemBar,
} from '@mui/material';
import { Video } from '../../models/Video';

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
          <img src={`data:image/png;base64, ${v.serializedImage}`}></img>
          <ImageListItemBar
            title={v.title}
            subtitle={v.length}
          ></ImageListItemBar>
        </ImageListItem>
      ))}
    </ImageList>
  );
};

export default VideosImageList;
