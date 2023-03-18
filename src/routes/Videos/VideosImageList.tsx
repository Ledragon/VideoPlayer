import { ImageList } from '@mui/material';
import { Video } from '../../models/Video';
import { Thumbnail } from '../../models/Thumbnail';
import VideoImageItem from './VideoImageItem';
import { useDispatch } from 'react-redux';
import { addVideo } from '../../redux/playlist.slice';

interface PropType {
  thumbnails: Thumbnail[];
  videos: Video[];
  onClick?: (v: Video) => void;
}

const VideosImageList = (props: PropType) => {
  const { thumbnails, videos, onClick } = props;
  const dispatch = useDispatch();

  const handleaAddToPlaylist = (v: Video) => {
    dispatch(addVideo(v.id));
  }

  return (
    <ImageList
      cols={6}
      gap={12}
      sx={{ m: 1 }}
    >
      {videos.map((v, i) => {
        const currentThumbs =
          thumbnails?.filter((t) => t.videoId === v.id) || [];
        return (
          <VideoImageItem
            key={i}
            video={v}
            image={currentThumbs.length ? currentThumbs[0].image : undefined}
            onInfoClick={onClick}
            onAddToPlaylist={handleaAddToPlaylist}
          />
        );
      })}
    </ImageList>
  );
};

export default VideosImageList;
