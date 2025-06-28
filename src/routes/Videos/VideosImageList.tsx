import { forwardRef } from 'react';
import { VirtuosoGrid } from 'react-virtuoso';

import { Video } from '../../models/Video';
import VideoImageItem from './VideoImageItem';

interface PropType {
  showContactSheets?: boolean;
  videos: Video[];
  onClick?: (v: Video) => void;
}

const VideoGridList = forwardRef<HTMLDivElement, { style?: React.CSSProperties; children?: React.ReactNode }>(({ style, children }, ref) => (
  <div
    ref={ref}
    style={{
      ...style,
      display: 'grid',
      gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
      gap: '1.5rem',
      padding: '1rem'
    }}>
    {children || <></>}
  </div>
));

VideoGridList.displayName = 'VideoGridList';

const VideosImageList = (props: PropType) => {
  const { videos, onClick, showContactSheets } = props;

  return (
    <VirtuosoGrid
      totalCount={videos.length}
      components={{
        List: VideoGridList
      }}
      itemContent={(index) => {
        return <VideoImageItem key={index} video={videos[index]} showContactSheet={showContactSheets} onInfoClick={onClick} />;
      }}
    />
  );
};

export default VideosImageList;
