import HomeButtonsFullScreen from './HomeButtonsFullScreen';

const buttons = [
  { label: 'Settings', route: '../settings' },
  { label: 'Manage', route: '../manage' },
  { label: 'Videos', route: '../videos' },
  { label: 'Pictures', route: '../pictures' }
];

const Home = () => {
  return (
    <HomeButtonsFullScreen buttons={buttons}/>
  );
};

export default Home;
