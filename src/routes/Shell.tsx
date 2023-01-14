import { Box } from '@mui/material';
import { Outlet, useLocation, useMatch } from 'react-router-dom';
import BottomNavigation from './BottomNavigation';

const Shell = () => {
  const location = useLocation();
  console.log(location);
  return (
    <Box
      sx={{
        display: 'flex',
        flexGrow: 1,
        height: '100%',
        flexDirection: 'column'
      }}
    >
      <Box sx={{ flexGrow: 1, flexDirection: 'column', display: 'flex' }}>
        <Outlet />
      </Box>
      <BottomNavigation value={location.pathname} />
    </Box>
  );
};

export default Shell;
