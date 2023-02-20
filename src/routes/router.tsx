import { createBrowserRouter, redirect } from 'react-router-dom';
import Home from './Home/Home';
import Pictures from './Pictures';
import Settings from './Settings';
import Shell from './Shell';
import VideoPage from './Video/VideoPage';
import VideosNewPage from './Videos/VideosNewPage';
import VideosPage from './Videos/VideosPage';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Shell />,
    children: [
      {
        path: '',
        loader: () => redirect('/home')
      },
      {
        path: '/home',
        id: 'home',
        element: <Home />,
        index: true
      },
      {
        path: '/videos',
        id: 'videos',
        // element: <VideosPage />
        element: <VideosNewPage />
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
      },
      {
        path: '/video/:id',
        id: 'video',
        element: <VideoPage />
      }
    ]
  }
]);

export default router;
