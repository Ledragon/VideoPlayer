import { createTheme, PaletteOptions, ThemeOptions } from '@mui/material/styles';

// Mantine's color palette
const mantineBlue = {
  50: '#e7f5ff',
  100: '#d0ebff',
  200: '#a5d8ff',
  300: '#74c0fc',
  400: '#339af0',
  500: '#228be6', // Primary
  600: '#1c7ed6',
  700: '#1971c2',
  800: '#1864ab',
  900: '#0b4d8c'
};

// Mantine spacing scale
const mantineSpacing = {
  xs: 10,
  sm: 12,
  md: 16,
  lg: 20,
  xl: 24
};

// Mantine border radius scale
const mantineRadius = {
  xs: 2,
  sm: 4,
  md: 8,
  lg: 16,
  xl: 32
};

// Mantine shadows
const mantineShadows = {
  xs: '0 1px 3px rgba(0, 0, 0, 0.05), 0 1px 2px rgba(0, 0, 0, 0.1)',
  sm: '0 1px 3px rgba(0, 0, 0, 0.05), 0 1px 2px rgba(0, 0, 0, 0.1)',
  md: '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
  lg: '0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)',
  xl: '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)'
};

const darkPalette: PaletteOptions = {
  mode: 'dark',
  primary: {
    main: mantineBlue[400],
    light: mantineBlue[300],
    dark: mantineBlue[600]
  },
  secondary: {
    main: '#868e96',
    light: '#adb5bd',
    dark: '#495057'
  },
  background: {
    default: '#1a1b1e',
    paper: '#25262b'
  },
  text: {
    primary: '#c1c2c5',
    secondary: '#909296'
  },
  divider: '#373a40',
  grey: {
    50: '#f8f9fa',
    100: '#f1f3f4',
    200: '#e9ecef',
    300: '#dee2e6',
    400: '#ced4da',
    500: '#adb5bd',
    600: '#868e96',
    700: '#495057',
    800: '#343a40',
    900: '#212529'
  }
};

const lightPalette: PaletteOptions = {
  mode: 'light',
  primary: {
    main: mantineBlue[600],
    light: mantineBlue[400],
    dark: mantineBlue[800]
  },
  secondary: {
    main: '#868e96',
    light: '#adb5bd',
    dark: '#495057'
  },
  background: {
    default: '#ffffff',
    paper: '#ffffff'
  },
  text: {
    primary: '#212529',
    secondary: '#495057'
  },
  divider: '#dee2e6',
  grey: {
    50: '#f8f9fa',
    100: '#f1f3f4',
    200: '#e9ecef',
    300: '#dee2e6',
    400: '#ced4da',
    500: '#adb5bd',
    600: '#868e96',
    700: '#495057',
    800: '#343a40',
    900: '#212529'
  }
};

const getMantineStyledTheme = (mode: 'light' | 'dark') => {
  const palette = mode === 'light' ? lightPalette : darkPalette;

  const theme: ThemeOptions = {
    palette,
    typography: {
      fontFamily: [
        '-apple-system',
        'BlinkMacSystemFont',
        '"Segoe UI"',
        'Roboto',
        '"Helvetica Neue"',
        'Arial',
        'sans-serif',
        '"Apple Color Emoji"',
        '"Segoe UI Emoji"',
        '"Segoe UI Symbol"'
      ].join(','),
      h1: {
        fontSize: '2.125rem',
        fontWeight: 700,
        lineHeight: 1.15
      },
      h2: {
        fontSize: '1.625rem',
        fontWeight: 700,
        lineHeight: 1.2
      },
      h3: {
        fontSize: '1.375rem',
        fontWeight: 600,
        lineHeight: 1.25
      },
      h4: {
        fontSize: '1.125rem',
        fontWeight: 600,
        lineHeight: 1.3
      },
      h5: {
        fontSize: '1rem',
        fontWeight: 600,
        lineHeight: 1.4
      },
      h6: {
        fontSize: '0.875rem',
        fontWeight: 600,
        lineHeight: 1.45
      },
      body1: {
        fontSize: '0.875rem',
        lineHeight: 1.55,
        fontWeight: 400
      },
      body2: {
        fontSize: '0.75rem',
        lineHeight: 1.6,
        fontWeight: 400
      },
      caption: {
        fontSize: '0.75rem',
        lineHeight: 1.6,
        fontWeight: 400
      }
    },
    spacing: (factor: number) => {
      // Map MUI spacing to Mantine spacing
      const spacingMap: { [key: number]: number } = {
        0: 0,
        0.5: mantineSpacing.xs / 2,
        1: mantineSpacing.xs,
        1.5: mantineSpacing.sm,
        2: mantineSpacing.md,
        2.5: mantineSpacing.lg,
        3: mantineSpacing.xl,
        4: mantineSpacing.xl + 8,
        5: mantineSpacing.xl + 16,
        6: mantineSpacing.xl + 24,
        8: mantineSpacing.xl + 40
      };
      return spacingMap[factor] || factor * 8;
    },
    shape: {
      borderRadius: mantineRadius.sm
    },
    shadows: [
      'none',
      mantineShadows.xs,
      mantineShadows.sm,
      mantineShadows.md,
      mantineShadows.md,
      mantineShadows.md,
      mantineShadows.lg,
      mantineShadows.lg,
      mantineShadows.lg,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl,
      mantineShadows.xl
    ],
    components: {
      MuiCssBaseline: {
        styleOverrides: {
          body: {
            fontFamily: ['-apple-system', 'BlinkMacSystemFont', '"Segoe UI"', 'Roboto', '"Helvetica Neue"', 'Arial', 'sans-serif'].join(',')
          }
        }
      },
      MuiButton: {
        styleOverrides: {
          root: {
            textTransform: 'none',
            fontWeight: 500,
            borderRadius: mantineRadius.sm,
            padding: '8px 16px',
            fontSize: '0.875rem',
            minHeight: 36,
            '&:hover': {
              transform: 'none'
            }
          },
          contained: {
            boxShadow: 'none',
            '&:hover': {
              boxShadow: 'none'
            }
          },
          outlined: {
            borderColor: mode === 'dark' ? '#373a40' : '#ced4da',
            '&:hover': {
              borderColor: mode === 'dark' ? '#495057' : '#adb5bd',
              backgroundColor: mode === 'dark' ? 'rgba(255, 255, 255, 0.05)' : 'rgba(0, 0, 0, 0.04)'
            }
          }
        }
      },
      MuiIconButton: {
        styleOverrides: {
          root: {
            borderRadius: mantineRadius.sm,
            '&:hover': {
              backgroundColor: mode === 'dark' ? 'rgba(255, 255, 255, 0.05)' : 'rgba(0, 0, 0, 0.04)'
            }
          }
        }
      },
      MuiPaper: {
        styleOverrides: {
          root: {
            borderRadius: mantineRadius.md,
            border: mode === 'dark' ? '1px solid #373a40' : '1px solid #dee2e6',
            boxShadow: mantineShadows.sm
          }
        }
      },
      MuiCard: {
        styleOverrides: {
          root: {
            borderRadius: mantineRadius.md,
            border: mode === 'dark' ? '1px solid #373a40' : '1px solid #dee2e6',
            boxShadow: mantineShadows.sm
          }
        }
      },
      MuiTextField: {
        styleOverrides: {
          root: {
            '& .MuiOutlinedInput-root': {
              borderRadius: mantineRadius.sm,
              fontSize: '0.875rem',
              '& fieldset': {
                borderColor: mode === 'dark' ? '#373a40' : '#ced4da'
              },
              '&:hover fieldset': {
                borderColor: mode === 'dark' ? '#495057' : '#adb5bd'
              },
              '&.Mui-focused fieldset': {
                borderWidth: '2px'
              }
            },
            '& .MuiInputLabel-root': {
              fontSize: '0.875rem',
              fontWeight: 500
            }
          }
        }
      },
      MuiAutocomplete: {
        styleOverrides: {
          root: {
            '& .MuiOutlinedInput-root': {
              borderRadius: mantineRadius.sm,
              fontSize: '0.875rem'
            }
          },
          paper: {
            borderRadius: mantineRadius.sm,
            border: mode === 'dark' ? '1px solid #373a40' : '1px solid #dee2e6',
            boxShadow: mantineShadows.md
          },
          option: {
            fontSize: '0.875rem',
            minHeight: 36,
            '&:hover': {
              backgroundColor: mode === 'dark' ? 'rgba(255, 255, 255, 0.05)' : 'rgba(0, 0, 0, 0.04)'
            }
          }
        }
      },
      MuiChip: {
        styleOverrides: {
          root: {
            borderRadius: mantineRadius.xl,
            fontSize: '0.75rem',
            fontWeight: 500,
            height: 24
          },
          outlined: {
            borderColor: mode === 'dark' ? '#373a40' : '#ced4da'
          }
        }
      },
      MuiDialog: {
        styleOverrides: {
          paper: {
            borderRadius: mantineRadius.md,
            border: mode === 'dark' ? '1px solid #373a40' : '1px solid #dee2e6'
          }
        }
      },
      MuiDialogTitle: {
        styleOverrides: {
          root: {
            fontSize: '1.125rem',
            fontWeight: 600,
            padding: '20px 24px 16px'
          }
        }
      },
      MuiDialogContent: {
        styleOverrides: {
          root: {
            padding: '0 24px',
            fontSize: '0.875rem'
          }
        }
      },
      MuiDialogActions: {
        styleOverrides: {
          root: {
            padding: '16px 24px 20px',
            gap: 8
          }
        }
      },
      MuiTab: {
        styleOverrides: {
          root: {
            textTransform: 'none',
            fontSize: '0.875rem',
            fontWeight: 500,
            minHeight: 40,
            borderRadius: `${mantineRadius.sm}px ${mantineRadius.sm}px 0 0`
          }
        }
      },
      MuiTabs: {
        styleOverrides: {
          indicator: {
            height: 2,
            borderRadius: '2px 2px 0 0'
          }
        }
      },
      MuiSwitch: {
        styleOverrides: {
          root: {
            '& .MuiSwitch-thumb': {
              boxShadow: mantineShadows.sm
            }
          }
        }
      },
      MuiRating: {
        styleOverrides: {
          root: {
            color: mantineBlue[400]
          }
        }
      },
      MuiBottomNavigation: {
        styleOverrides: {
          root: {
            borderTop: mode === 'dark' ? '1px solid #373a40' : '1px solid #dee2e6',
            backgroundColor: mode === 'dark' ? '#25262b' : '#ffffff'
          }
        }
      },
      MuiBottomNavigationAction: {
        styleOverrides: {
          root: {
            fontSize: '0.75rem',
            fontWeight: 500,
            '&.Mui-selected': {
              color: mantineBlue[mode === 'dark' ? 400 : 600]
            }
          }
        }
      },
      MuiSpeedDial: {
        styleOverrides: {
          fab: {
            borderRadius: mantineRadius.xl,
            '&:hover': {
              transform: 'none'
            }
          }
        }
      },
      MuiSpeedDialAction: {
        styleOverrides: {
          fab: {
            borderRadius: mantineRadius.lg,
            '&:hover': {
              transform: 'none'
            }
          }
        }
      }
    }
  };

  return createTheme(theme);
};

export default getMantineStyledTheme;
