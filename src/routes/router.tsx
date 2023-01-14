import { createBrowserRouter } from 'react-router-dom';
import Home from './Home';
import Pictures from './Pictures';
import Settings from './Settings';
import Shell from './Shell';
import VideosPage from './Videos/VideosPage';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Shell />,
    children: [
      {
        path: '/home',
        id: 'home',
        element: <Home />,
        index: true
      },
      {
        path: '/videos',
        id: 'videos',
        element: <VideosPage />
      },
      {
        path: '/settings',
        id: 'settings',
        element: <Settings />
      },
      {
        path: '/pictures',
        id: 'pictures',
        element: <Pictures />
      }
    ]
  }
]);

export default router;
