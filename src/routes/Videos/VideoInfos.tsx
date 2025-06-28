import { Box, Chip, Rating } from '@mui/material';

import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import ThumbsDisplay from './ThumbsDisplay';

interface PropType {
  tags?: (Tag | undefined)[];
  video: Video;
}

const VideoInfos = (props: PropType) => {
  const { tags, video } = props;
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
        <ThumbsDisplay video={video} />
      </Box>
    </Box>
  );
};

export default VideoInfos;
