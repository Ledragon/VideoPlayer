import HomeButtonsStack from './HomeButtonsStack';
import linksDefinition from '../linksDefinition';

const buttons = linksDefinition;
const Home = () => {
  return <HomeButtonsStack buttons={buttons} />;
};

export default Home;
