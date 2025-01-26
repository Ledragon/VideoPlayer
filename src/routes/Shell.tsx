import { Box } from '@mui/material';
import { useEffect, useMemo, useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';

import { useGetThumbnailsQuery } from '../redux';
import BottomNavigation from './BottomNavigation';
import getBase64Url from './Videos/getBase64Url';

const Shell = () => {
  const location = useLocation();

  const [background, setBackground] = useState('/TheaterCurtains.jpg');

  const { data: thumbs } = useGetThumbnailsQuery();

  const filtered = useMemo(() => {
    return thumbs?.filter((video) => video.image) || [];
  }, [thumbs]);

  useEffect(() => {
    if (filtered?.length) {
      const id = setInterval(() => {
        const video = filtered[Math.floor(Math.random() * filtered.length)];

        setBackground(getBase64Url(video.image));
      }, 5000);
      return () => clearInterval(id);
    }
  }, [filtered]);

  return (
    <Box
      sx={{
        display: 'flex',
        flexGrow: 1,
        height: '100%',
        flexDirection: 'column',
        backgroundImage: `url("${background}")`,
        backgroundSize: 'cover',
        backgroundrepeat: 'no-repeat'
      }}
      id="shell">
      <Box
        sx={{
          flexGrow: 1,
          flexDirection: 'column',
          display: 'flex',
          flexShrink: 1,
          overflow: 'auto'
        }}
        id="router-outlet">
        <Outlet />
      </Box>
      <Box sx={{ flexBasis: 'auto', flexShrink: 0, flexGrow: 0 }}>
        <BottomNavigation value={location.pathname} />
      </Box>
    </Box>
  );
};

export default Shell;
