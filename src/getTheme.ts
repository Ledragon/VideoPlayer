import { createTheme, PaletteOptions } from '@mui/material';
import {
  blueGrey,
  deepPurple,
  grey,
  lightBlue,
  teal
} from '@mui/material/colors';

const darkPalette: PaletteOptions = {
  primary: { main: lightBlue[400] }
  // grey: blueGrey
};

const lightPalette: PaletteOptions = {
  background: { default: grey[100] },
  primary: { main: deepPurple[700] }
};

const getTheme = (theme: 'light' | 'dark') =>
  createTheme({
    palette: Object.assign(
      {},
      {
        mode: theme
      },
      theme === 'light' ? lightPalette : darkPalette
    )
  });

export default getTheme;
