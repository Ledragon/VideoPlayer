import { Box, Grid, Typography } from '@mui/material';

import { Format } from '../../models/FFProbeVideoInfo/Format';
import { formatDuration } from './formatDuration';
import { humanFileSize } from './humanFileSize';

interface PropType {
  format: Format;
}

const FormatInfo = (props: PropType) => {
  const { format } = props;

  return (
    <Box>
      <Typography variant="h6">File Information</Typography>
      <Grid container spacing={1} sx={{ mt: 1 }}>
        <Grid item xs={6}>
          <Typography variant="body2">Duration:</Typography>
        </Grid>
        <Grid item xs={6}>
          <Typography variant="body2" align="left">
            {formatDuration(+format.duration)}
          </Typography>
        </Grid>

        <Grid item xs={6}>
          <Typography variant="body2">Size:</Typography>
        </Grid>
        <Grid item xs={6}>
          <Typography variant="body2" align="left">
            {humanFileSize(+format.size)}
          </Typography>
        </Grid>
        <Grid item xs={6}>
          <Typography variant="body2">Format:</Typography>
        </Grid>
        <Grid item xs={6}>
          <Typography variant="body2" align="left">
            {format.format_name}
          </Typography>
        </Grid>

        <Grid item xs={6}>
          <Typography variant="body2">Format Long Name:</Typography>
        </Grid>
        <Grid item xs={6}>
          <Typography variant="body2" align="left">
            {format.format_long_name}
          </Typography>
        </Grid>

        <Grid item xs={6}>
          <Typography variant="body2">Number of Programs:</Typography>
        </Grid>
        <Grid item xs={6}>
          <Typography variant="body2" align="left">
            {format.nb_programs}
          </Typography>
        </Grid>

        <Grid item xs={6}>
          <Typography variant="body2">Number of Streams:</Typography>
        </Grid>
        <Grid item xs={6}>
          <Typography variant="body2" align="left">
            {format.nb_streams}
          </Typography>
        </Grid>
      </Grid>
    </Box>
  );
};

export default FormatInfo;
