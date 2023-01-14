import HomeButtonsFullScreen from './HomeButtonsFullScreen';
import HomeButtonsStack from './HomeButtonsStack';

const buttons = [
  { label: 'Settings', route: '../settings' },
  { label: 'Manage', route: '../manage' },
  { label: 'Videos', route: '../videos' },
  { label: 'Pictures', route: '../pictures' }
];

const Home = () => {
  return (
    <HomeButtonsStack buttons={buttons}/>
  );
};

export default Home;
