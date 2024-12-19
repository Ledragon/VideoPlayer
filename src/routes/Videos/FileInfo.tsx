import { Stack, Typography } from '@mui/material';

import { Video } from '../../models/Video';
import { useMetadataQuery } from '../../redux';

interface PropType {
  video: Video;
}

const FileInfo = (props: PropType) => {
  const { video } = props;
  const { data } = useMetadataQuery(video.id);
  console.log(data);
  const format = data?.format;

  // create a component that displays properties from format using the typography component

  return (
    <Stack direction="column">
      {format && (
        <>
          <Typography variant="h6">File Information</Typography>
          <Typography variant="body2">Duration: {format.duration}</Typography>
          <Typography variant="body2">Size: {format.size}</Typography>
          <Typography variant="body2">Bitrate: {format.bit_rate}</Typography>
        </>
      )}
    </Stack>
  );
};

export default FileInfo;
