import { ImageList } from '@mui/material';
import { Video } from '../../models/Video';
import VideoImageItem from './VideoImageItem';
import { useDispatch } from 'react-redux';
import { addVideo } from '../../redux/playlist.slice';
import { VirtuosoGrid } from 'react-virtuoso';
import { forwardRef } from 'react';

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
    <VirtuosoGrid
      totalCount={videos.length}
      components={{
        List: forwardRef(({ style, children }, ref) => {
          return (
            <ImageList
              cols={4}
              gap={12}
              rowHeight={250}
              style={{ ...style }}
              ref={ref}
              component={'div'}
            >
              {children || <></>}
            </ImageList>
          );
        })
      }}
      itemContent={(index, data) => {
        return (
          <VideoImageItem
            key={index}
            video={videos[index]}
            onInfoClick={onClick}
            onAddToPlaylist={handleaAddToPlaylist}
          />
        );
      }}
    ></VirtuosoGrid>
  );
};

export default VideosImageList;
