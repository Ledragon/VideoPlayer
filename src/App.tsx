import './App.css';

import { CssBaseline, ThemeProvider } from '@mui/material';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';

import getTheme from './getTheme';
import { store } from './redux/configureStore';
import router from './routes/router';

const App = () => {
  return (
    <Provider store={store}>
      <ThemeProvider theme={getTheme('dark')}>
        <CssBaseline />
        <RouterProvider router={router} />
      </ThemeProvider>
    </Provider>
  );
};

export default App;
