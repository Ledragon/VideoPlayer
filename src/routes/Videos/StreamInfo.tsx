import { Box, Typography } from '@mui/material';

import { Stream } from '../../models/FFProbeVideoInfo/Stream';

interface PropType {
  stream: Stream;
}

const StreamInfo = (props: PropType) => {
  const { stream } = props;

  // Display all stream information using Typography and body2 variant
  return (
    <Box>
      <Typography variant="body2">Index: {stream.index}</Typography>
      <Typography variant="body2">Codec: {stream.codec_name}</Typography>
      <Typography variant="body2">Codec Long Name: {stream.codec_long_name}</Typography>
      <Typography variant="body2">Codec Type: {stream.codec_type}</Typography>
      <Typography variant="body2">Codec Tag: {stream.codec_tag}</Typography>
      <Typography variant="body2">Codec Tag String: {stream.codec_tag_string}</Typography>
      <Typography variant="body2">Bitrate: {stream.bit_rate}</Typography>
      <Typography variant="body2">Duration: {stream.duration}</Typography>
      <Typography variant="body2">Width: {stream.width}</Typography>
      <Typography variant="body2">Height: {stream.height}</Typography>
      <Typography variant="body2">Sample Rate: {stream.sample_rate}</Typography>
      <Typography variant="body2">Sample Format: {stream.sample_fmt}</Typography>
      <Typography variant="body2">Channel Layout: {stream.channel_layout}</Typography>
      <Typography variant="body2">Channels: {stream.channels}</Typography>
      <Typography variant="body2">Pix Format: {stream.pix_fmt}</Typography>
      <Typography variant="body2">Time Base: {stream.time_base}</Typography>
      <Typography variant="body2">Start PTS: {stream.start_pts}</Typography>
      <Typography variant="body2">Start Time: {stream.start_time}</Typography>
      <Typography variant="body2">Duration TS: {stream.duration_ts}</Typography>
    </Box>
  );
};

export default StreamInfo;
