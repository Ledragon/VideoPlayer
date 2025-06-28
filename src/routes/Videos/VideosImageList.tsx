import { ImageList } from '@mui/material';
import { forwardRef } from 'react';
import { VirtuosoGrid } from 'react-virtuoso';

import { Video } from '../../models/Video';
import VideoImageItem from './VideoImageItem';

interface PropType {
  showContactSheets?: boolean;
  videos: Video[];
  onClick?: (v: Video) => void;
}

const VideosImageList = (props: PropType) => {
  const { videos, onClick, showContactSheets } = props;

  return (
    <VirtuosoGrid
      totalCount={videos.length}
      components={{
        List: forwardRef(({ style, children }, ref) => {
          return (
            <ImageList cols={4} gap={24} rowHeight={180} style={{ ...style }} ref={ref} component={'div'}>
              {children || <></>}
            </ImageList>
          );
        })
      }}
      itemContent={(index, data) => {
        return <VideoImageItem key={index} video={videos[index]} showContactSheet={showContactSheets} onInfoClick={onClick} />;
      }}></VirtuosoGrid>
  );
};

export default VideosImageList;
