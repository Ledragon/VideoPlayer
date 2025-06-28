import { SimpleGrid } from '@mantine/core';
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
            <SimpleGrid cols={{ base: 1, sm: 2, md: 3, lg: 4, xl: 5 }} spacing="lg" style={{ ...style, padding: '1rem' }} ref={ref}>
              {children || <></>}
            </SimpleGrid>
          );
        })
      }}
      itemContent={(index, data) => {
        return <VideoImageItem key={index} video={videos[index]} showContactSheet={showContactSheets} onInfoClick={onClick} />;
      }}
    />
  );
};

export default VideosImageList;
