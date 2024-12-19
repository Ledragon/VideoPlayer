import { Stack, Typography } from '@mui/material';

import { Video } from '../../models/Video';
import { useMetadataQuery } from '../../redux';
import FormatInfo from './FormatInfo';
import StreamInfo from './StreamInfo';

interface PropType {
  video: Video;
}

const FileInfo = (props: PropType) => {
  const { video } = props;
  const { data } = useMetadataQuery(video.id);
  console.log(data);
  const format = data?.format;
  const streams = data?.streams;

  return (
    <Stack direction="column">
      {format && (
        <>
          {format && <FormatInfo format={format} />}
          <Typography variant="h6">Streams</Typography>
          {streams?.map((stream, i) => <StreamInfo key={i} stream={stream} />)}
        </>
      )}
    </Stack>
  );
};

export default FileInfo;
