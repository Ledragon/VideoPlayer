import { Box, Chip, Rating, Stack, Typography } from '@mui/material';

import { Tag } from '../../models/Tag';
import { Video } from '../../models/Video';
import { useGetThumbnailsForVideoQuery } from '../../redux';
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
          <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', flexGrow: 1 }}>
            <img src={'/no-image-14600.svg'} alt="no image" />
            <Typography variant="body1">No thumbnail for this video</Typography>
          </Box>
        )}
      </Box>
    </Box>
  );
};

export default VideoInfos;
