import { Box, Chip, Rating } from '@mui/material';

import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import { useGetThumbnailsForVideoQuery } from '../../redux';
import EmptyThumbnails from './EmptyThumbnails';
import ThumbsImageList from './ThumbsImageList';

interface PropType {
  tags?: (Tag | undefined)[];
  video: Video;
}

const VideoInfos = (props: PropType) => {
  const { tags, video } = props;
  const { data: thumbnails } = useGetThumbnailsForVideoQuery(video.id);

  return (
    <Box sx={{ flexGrow: 1, height: '100%' }} id="video-infos">
      <Box
        sx={{
          display: 'flex',
          flexDirection: 'column',
          flexGrow: 1,
          height: '100%',
          overflow: 'auto'
        }}>
        <Box>{tags?.map((tag) => <Chip key={tag?.id} label={tag?.value} variant={'outlined'} sx={{ m: 1 }} />)}</Box>
        <Rating value={video.rating} readOnly />
        {thumbnails?.length ? (
          <ThumbsImageList sx={{ flexGrow: 1 }} cols={thumbnails ? Math.min(thumbnails?.length, 4) : 0} thumbs={thumbnails || []} />
        ) : (
          <EmptyThumbnails />
        )}
      </Box>
    </Box>
  );
};

export default VideoInfos;
