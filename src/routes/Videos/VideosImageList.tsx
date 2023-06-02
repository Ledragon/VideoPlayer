import { ImageList } from '@mui/material';
import { Video } from '../../models/Video';
import { Thumbnail } from '../../models/Thumbnail';
import VideoImageItem from './VideoImageItem';
import { useDispatch } from 'react-redux';
import { addVideo } from '../../redux/playlist.slice';

interface PropType {
  // thumbnails: Thumbnail[];
  videos: Video[];
  onClick?: (v: Video) => void;
}

const VideosImageList = (props: PropType) => {
  const { videos, onClick } = props;
  const dispatch = useDispatch();

  const handleaAddToPlaylist = (v: Video) => {
    dispatch(addVideo(v.id));
  };

  return (
    <ImageList
      cols={4}
      gap={12}
      sx={{}}
      rowHeight={200}
    >
      {videos?.map((v, i) => {
        // const currentThumbs =
        //   thumbnails?.filter((t) => t.videoId === v.id) || [];
        return (
          <VideoImageItem
            key={i}
            video={v}
            onInfoClick={onClick}
            onAddToPlaylist={handleaAddToPlaylist}
          />
        );
      })}
    </ImageList>
  );
};

export default VideosImageList;
