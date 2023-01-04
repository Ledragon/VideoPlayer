import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import './App.css';
import Home from './routes/Home';
import VideoList from './routes/VideoList';

const router = createBrowserRouter([
  {
    path: '/home',
    element: <Home />,
  },{
    path: '/videos',
    element: <VideoList />,
  },
]);

function App() {
  return <RouterProvider router={router}/>;
}

export default App;
