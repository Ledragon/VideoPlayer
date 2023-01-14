import { createTheme, PaletteOptions } from '@mui/material';
import { blueGrey, deepPurple, lightBlue, teal } from '@mui/material/colors';

const darkPalette: PaletteOptions = {
  primary: { main: teal[300] },
  grey: blueGrey
};

const lightPalette: PaletteOptions = {
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
