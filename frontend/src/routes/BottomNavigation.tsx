import { BottomNavigation as MuiBottomNavigation, BottomNavigationAction } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

import linksDefinition from './linksDefinition';

const BottomNavigation = (props: { value: string }) => {
  const { value } = props;
  return (
    <MuiBottomNavigation value={value}>
      {linksDefinition.map((l, i) => {
        return (
          <BottomNavigationAction
            key={i}
            label={l.label}
            value={'/' + l.name}
            icon={l.icon}
            component={RouterLink}
            to={'../' + l.name}></BottomNavigationAction>
        );
      })}
    </MuiBottomNavigation>
  );
};

export default BottomNavigation;
