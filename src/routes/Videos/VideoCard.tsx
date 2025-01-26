import InsertPhotoIcon from '@mui/icons-material/InsertPhoto';
import { Box, Card, CardContent, CardHeader, CardMedia, Typography } from '@mui/material';

import { Video } from '../../models/Video';
import getBase64Url from './getBase64Url';

const VideoCard = (props: { video: Video }) => {
  const { video } = props;
  return (
    <Card>
      {/* <CardHeader title={selectedVideo?.title}></CardHeader> */}
      {!video.serializedImage && (
        <CardMedia component="img" width={320} height={240} src={'/no-image-14600.svg'} alt={video.title}>
          <InsertPhotoIcon />
        </CardMedia>
      )}
      {video.serializedImage && <CardMedia component="img" width={320} height={240} src={getBase64Url(video.serializedImage)} alt={video.title}></CardMedia>}
      <CardContent>
        <Typography variant="h5">{video.title}</Typography>
        <Typography variant="caption">{video.fileName}</Typography>
        <br />
        <Typography variant="caption">{video.numberOfViews}</Typography>
      </CardContent>
    </Card>
  );
};

export default VideoCard;
