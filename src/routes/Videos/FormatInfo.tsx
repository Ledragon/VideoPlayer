import { Box, Typography } from '@mui/material';

import { Format } from '../../models/FFProbeVideoInfo/Format';

interface PropType {
  format: Format;
}

const FormatInfo = (props: PropType) => {
  const { format } = props;
  return (
    <Box>
      <Typography variant="h6">File Information</Typography>
      <Typography variant="body2">Duration: {format.duration}</Typography>
      <Typography variant="body2">Size: {format.size}</Typography>
      <Typography variant="body2">Bitrate: {format.bit_rate}</Typography>
      <Typography variant="body2">Format: {format.format_name}</Typography>
      <Typography variant="body2">Format Long Name: {format.format_long_name}</Typography>
      <Typography variant="body2">Number of Programs: {format.nb_programs}</Typography>
      <Typography variant="body2">Number of Streams: {format.nb_streams}</Typography>
      <Typography variant="body2">Probe Score: {format.probe_score}</Typography>
      <Typography variant="body2">Start Time: {format.start_time}</Typography>
    </Box>
  );
};

export default FormatInfo;
