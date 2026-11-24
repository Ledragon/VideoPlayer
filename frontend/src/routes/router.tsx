import { createBrowserRouter, redirect } from 'react-router-dom';

import Home from './Home/Home';
import Pictures from './Pictures';
import PlaylistPage from './Playlist/PlaylistPage';
import SettingsPage from './Settings/SettingsPage';
import Shell from './Shell';
import VideoPage from './Video/VideoPage';
import VideosNewPage from './Videos/VideosNewPage';

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
        element: <SettingsPage />
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
      },
      {
        path: '/playlist',
        id: 'playlist',
        // element: <VideosPage />
        element: <PlaylistPage />
      }
    ]
  }
]);

export default router;
