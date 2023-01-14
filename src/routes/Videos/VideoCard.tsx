import { Box, Card, CardHeader, CardMedia } from '@mui/material';
import { Video } from '../../models/Video';

const VideoCard = (props: { selectedVideo: Video }) => {
  const { selectedVideo } = props;
  return (
    <Card>
      <CardHeader title={selectedVideo?.title}></CardHeader>
      <CardMedia
        component="img"
        height={540}
        src={`data:image/png;base64, ${selectedVideo.serializedImage}`}
        alt={selectedVideo.title}
      ></CardMedia>
    </Card>
  );
};

export default VideoCard;
