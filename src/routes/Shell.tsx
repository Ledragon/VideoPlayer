import { Paper } from '@mui/material';
import { Outlet } from 'react-router-dom';

const Shell = () => {
  return (
    <Paper sx={{ display: 'flex', flexGrow: 1, height: '100%' }}>
      <Outlet />
    </Paper>
  );
};

export default Shell;
