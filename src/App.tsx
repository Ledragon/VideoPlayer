import { CssBaseline, ThemeProvider } from '@mui/material';
import { RouterProvider } from 'react-router-dom';

import './App.css';
import getTheme from './getTheme';
import router from './routes/router';

function App() {
  return (
    <ThemeProvider theme={getTheme('dark')}>
      <CssBaseline />
      <RouterProvider router={router} />
    </ThemeProvider>
  );
}

export default App;
