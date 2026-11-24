import { Box } from '@mui/material';
import { Outlet, useLocation } from 'react-router-dom';

import ConnectionStatus from '../components/ConnectionStatus';
import useGetRandomThumb from '../hooks/useGetRandomThumb';
import BottomNavigation from './BottomNavigation';

const Shell = () => {
  const location = useLocation();

  const background = '/TheaterCurtains.jpg';
  // const background = useGetRandomThumb();

  return (
    <Box
      sx={{
        display: 'flex',
        flexGrow: 1,
        height: '100%',
        flexDirection: 'column',
        backgroundImage: `url("${background}")`,
        backgroundSize: 'cover',
        backgroundRepeat: 'no-repeat',
        backgroundPosition: 'center'
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
        {/* <ConnectionStatus className="connection-status" /> */}
        <Outlet />
      </Box>
      <Box sx={{ flexBasis: 'auto', flexShrink: 0, flexGrow: 0 }}>
        <BottomNavigation value={location.pathname} />
      </Box>
    </Box>
  );
};

export default Shell;
