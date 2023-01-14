import { createBrowserRouter } from 'react-router-dom';
import Home from './Home';
import Pictures from './Pictures';
import Settings from './Settings';
import Shell from './Shell';
import VideoList from './VideoList';

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
        element: <VideoList />
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
