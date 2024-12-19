import { Box } from '@mui/material';
import { useRef } from 'react';

interface VideoPlayerProps {
  videoId: number;
}

export const VideoPlayer = ({ videoId }: VideoPlayerProps) => {
  const videoRef = useRef<HTMLVideoElement>(null);

  return (
    <Box
      sx={{
        width: '100%',
        height: '100%',
        display: 'flex',
        justifyContent: 'center'
      }}>
      <video ref={videoRef} controls style={{ maxWidth: '100%', maxHeight: '100%' }}>
        <source src={`http://localhost:5000/api/stream/transcode/${videoId}`} type="video/mp4" />
      </video>
    </Box>
  );
};
