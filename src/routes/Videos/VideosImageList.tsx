import { ImageList } from '@mui/material';
import { Video } from '../../models/Video';
import VideoImageItem from './VideoImageItem';
import { useDispatch } from 'react-redux';
import { addVideo } from '../../redux/playlist.slice';

interface PropType {
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
      cols={3}
      gap={12}
      sx={{}}
      rowHeight={250}
    >
      {videos?.map((v, i) => {
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
