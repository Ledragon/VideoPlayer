import { createBrowserRouter } from 'react-router-dom';
import Home from './Home';
import Shell from './Shell';
import VideoList from './VideoList';

const router = createBrowserRouter([
  {
    path: '/',
    element: <Shell />,
    children: [
      {
        path: '/home',
        element: <Home />,
        index: true
      },
      {
        path: '/videos',
        element: <VideoList />
      }
    ]
  }
]);

export default router;
