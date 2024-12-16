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
  return (
    <Stack direction="column">
      <Typography variant="caption" noWrap sx={{ m: 1 }}>
        {video.fileName}
      </Typography>
      <Typography variant="caption" sx={{ m: 1 }}>
        {video.length}
      </Typography>
    </Stack>
  );
};

export default FileInfo;
