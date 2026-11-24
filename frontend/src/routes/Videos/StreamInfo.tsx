import { Box, Typography } from '@mui/material';

import { Stream } from '../../models/FFProbeVideoInfo/Stream';
import { formatDuration } from './formatDuration';

interface PropType {
  stream: Stream;
}

const StreamInfo = (props: PropType) => {
  const { stream } = props;

  // Display all stream information using Typography and body2 variant
  return (
    <Box sx={{ m: 0.25 }}>
      <Typography variant="body2">Codec Type: {stream.codec_type}</Typography>
      <Typography variant="body2">Codec: {stream.codec_name}</Typography>
      <Typography variant="body2">Codec Long Name: {stream.codec_long_name}</Typography>
      <Typography variant="body2">Duration: {formatDuration(+stream.duration)}</Typography>
      <Typography variant="body2">Width: {stream.width}</Typography>
      <Typography variant="body2">Height: {stream.height}</Typography>
    </Box>
  );
};

export default StreamInfo;
