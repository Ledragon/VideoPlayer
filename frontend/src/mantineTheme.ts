import { createTheme, MantineColorsTuple } from '@mantine/core';

// Mantine blue color tuple - using the standard Mantine blue
const mantineBlue: MantineColorsTuple = ['#e7f5ff', '#d0ebff', '#a5d8ff', '#74c0fc', '#339af0', '#228be6', '#1c7ed6', '#1971c2', '#1864ab', '#145aa0'];

const mantineTheme = createTheme({
  /** Primary color - using Mantine's standard blue */
  primaryColor: 'blue',

  /** Custom colors - override with standard Mantine blue */
  colors: {
    blue: mantineBlue
  },

  /** Font family - Mantine defaults */
  fontFamily: '-apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica, Arial, sans-serif, Apple Color Emoji, Segoe UI Emoji',
  fontFamilyMonospace: 'ui-monospace, SFMono-Regular, SF Mono, Monaco, Consolas, Liberation Mono, Courier New, monospace',

  /** Font sizes - Mantine defaults */
  fontSizes: {
    xs: '0.75rem', // 12px
    sm: '0.875rem', // 14px
    md: '1rem', // 16px
    lg: '1.125rem', // 18px
    xl: '1.25rem' // 20px
  },

  /** Headings - Mantine defaults */
  headings: {
    fontFamily: '-apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica, Arial, sans-serif',
    fontWeight: '700',
    sizes: {
      h1: { fontSize: '2.125rem', lineHeight: '1.3' },
      h2: { fontSize: '1.625rem', lineHeight: '1.35' },
      h3: { fontSize: '1.375rem', lineHeight: '1.4' },
      h4: { fontSize: '1.125rem', lineHeight: '1.45' },
      h5: { fontSize: '1rem', lineHeight: '1.5' },
      h6: { fontSize: '0.875rem', lineHeight: '1.5' }
    }
  },

  /** Spacing - Mantine defaults */
  spacing: {
    xs: '0.625rem', // 10px
    sm: '0.75rem', // 12px
    md: '1rem', // 16px
    lg: '1.25rem', // 20px
    xl: '1.5rem' // 24px
  },

  /** Border radius - Mantine defaults */
  radius: {
    xs: '0.125rem', // 2px
    sm: '0.25rem', // 4px
    md: '0.5rem', // 8px
    lg: '1rem', // 16px
    xl: '2rem' // 32px
  },

  /** Default radius - Mantine default */
  defaultRadius: 'sm',

  /** Shadows - Mantine defaults */
  shadows: {
    xs: '0 0.0625rem 0.1875rem rgba(0, 0, 0, 0.05), 0 0.0625rem 0.125rem rgba(0, 0, 0, 0.1)',
    sm: '0 0.0625rem 0.1875rem rgba(0, 0, 0, 0.05), 0 0.0625rem 0.125rem rgba(0, 0, 0, 0.1)',
    md: '0 0.25rem 0.375rem -0.0625rem rgba(0, 0, 0, 0.1), 0 0.125rem 0.25rem -0.0625rem rgba(0, 0, 0, 0.06)',
    lg: '0 0.625rem 0.9375rem -0.1875rem rgba(0, 0, 0, 0.1), 0 0.25rem 0.375rem -0.125rem rgba(0, 0, 0, 0.05)',
    xl: '0 1.25rem 1.5625rem -0.3125rem rgba(0, 0, 0, 0.1), 0 0.625rem 0.625rem -0.3125rem rgba(0, 0, 0, 0.04)'
  },

  /** Other theme properties - Mantine defaults */
  focusRing: 'auto',
  respectReducedMotion: true,
  cursorType: 'default',

  /** Minimal component overrides - let Mantine handle most styling */
  components: {
    Paper: {
      defaultProps: {
        shadow: 'sm'
      }
    },

    Button: {
      defaultProps: {
        variant: 'filled'
      }
    },

    ActionIcon: {
      defaultProps: {
        variant: 'subtle'
      }
    },

    TextInput: {
      defaultProps: {
        size: 'sm'
      }
    },

    MultiSelect: {
      defaultProps: {
        size: 'sm'
      }
    },

    Switch: {
      defaultProps: {
        size: 'sm'
      }
    },

    Badge: {
      defaultProps: {
        variant: 'light'
      }
    },

    Tooltip: {
      defaultProps: {
        withArrow: true
      }
    }
  }
});

export default mantineTheme;
