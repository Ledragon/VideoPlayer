import 'video.js/dist/video-js.css';

import { Box } from '@mui/material';
import { useEffect, useRef } from 'react';
import videojs from 'video.js';
import Player from 'video.js/dist/types/player';

import { useMetadataQuery } from '../../redux';

interface VideoPlayerProps {
  videoId: number;
}

export const VideoPlayer = ({ videoId }: VideoPlayerProps) => {
  const videoRef = useRef<HTMLDivElement>(null);
  const playerRef = useRef<Player | null>(null);

  useEffect(() => {
    if (!playerRef.current && videoRef.current) {
      const videoElement = document.createElement('video-js');

      videoElement.classList.add('vjs-big-play-centered');
      videoRef.current.appendChild(videoElement);
      playerRef.current = videojs(videoElement, {
        controls: true,
        fluid: true,
        responsive: true,
        // preload: 'none',
        sources: [
          {
            src: `http://localhost:5000/api/stream/transcode/${videoId}`,
            type: 'video/mp4'
          }
        ]
      });
    }

    return () => {
      // if (playerRef.current) {
      //   playerRef.current.dispose();
      // }
    };
  }, [videoId, videoRef]);

  return (
    <Box
      sx={{
        width: '100%',
        height: '100%',
        display: 'flex',
        justifyContent: 'center'
      }}>
      <div data-vjs-player style={{ width: '100%', height: '100%', flexGrow: 1 }}>
        <div ref={videoRef} />
        {/* <video ref={videoRef} className="video-js vjs-big-play-centered" data-setup={{}} /> */}
      </div>
    </Box>
  );
};
