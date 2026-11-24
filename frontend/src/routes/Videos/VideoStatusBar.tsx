import { Box, Typography } from '@mui/material';

import { Video } from '../../models/Video';

interface PropType {
  video: Video;
}

const VideoStatusBar = (props: PropType) => {
  const { video } = props;

  return (
    <Box id="info-bar" sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', p: 0.5 }}>
      <Typography variant="caption">{video.fileName}</Typography>
      <Typography variant="caption">{video.length}</Typography>
    </Box>
  );
};

export default VideoStatusBar;
