import { Box, Typography } from '@mui/material';

import { Video } from '../../models/Video';
import { useGetThumbnailsForVideoQuery } from '../../redux';
import ThumbsImageList from './ThumbsImageList';

interface PropType {
  video: Video;
}

const ThumbsDisplay = (props: PropType) => {
  const { video } = props;
  const { data: thumbnails } = useGetThumbnailsForVideoQuery(video.id);

  return (
    <>
      {thumbnails?.length ? (
        <ThumbsImageList sx={{ flexGrow: 1 }} cols={thumbnails ? Math.min(thumbnails?.length, 4) : 0} thumbs={thumbnails || []} />
      ) : (
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', flexGrow: 1 }}>
          <img src={'/no-image-14600.svg'} alt="no image" />
          <Typography variant="body1">No thumbnail for this video</Typography>
        </Box>
      )}
    </>
  );
};

export default ThumbsDisplay;
