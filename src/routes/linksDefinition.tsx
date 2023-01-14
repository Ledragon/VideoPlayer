import VideoLibraryIcon from '@mui/icons-material/VideoLibrary';
import HomeIcon from '@mui/icons-material/Home';
import CollectionsIcon from '@mui/icons-material/Collections';
import TuneIcon from '@mui/icons-material/Tune';

const linksDefinition = [
  { label: 'Home', icon: <HomeIcon />, name: 'home' },
  {
    label: 'Videos',
    icon: <VideoLibraryIcon />,
    name: 'videos'
  },
  {
    label: 'Pictures',
    icon: <CollectionsIcon />,
    name: 'pictures'
  },
  { label: 'Settings', icon: <TuneIcon />, name: 'settings' }
  // { label: 'Manage', route: '../manage' },
];
export default linksDefinition;
