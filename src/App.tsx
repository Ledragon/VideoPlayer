import './App.css';
// Import Mantine core styles
import '@mantine/core/styles.css';
import '@mantine/notifications/styles.css';

import { MantineProvider } from '@mantine/core';
import { Notifications } from '@mantine/notifications';
import { CssBaseline, ThemeProvider } from '@mui/material';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';

import getMantineStyledTheme from './getMantineStyledTheme';
import mantineTheme from './mantineTheme';
import { store } from './redux/configureStore';
import router from './routes/router';

const App = () => {
  return (
    <Provider store={store}>
      <MantineProvider theme={mantineTheme} forceColorScheme="dark">
        <Notifications />
        <ThemeProvider theme={getMantineStyledTheme('dark')}>
          <CssBaseline />
          <RouterProvider router={router} />
        </ThemeProvider>
      </MantineProvider>
    </Provider>
  );
};

export default App;
