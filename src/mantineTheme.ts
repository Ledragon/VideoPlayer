import { createTheme, MantineColorsTuple } from '@mantine/core';

// Mantine blue color tuple (matching getMantineStyledTheme.ts)
const mantineBlue: MantineColorsTuple = [
  '#e7f5ff',
  '#d0ebff',
  '#a5d8ff',
  '#74c0fc',
  '#339af0',
  '#228be6',
  '#1c7ed6',
  '#1971c2',
  '#1864ab',
  '#0b4d8c'
];

const mantineTheme = createTheme({
  /** Primary color - using our custom blue */
  primaryColor: 'blue',
  
  /** Custom colors */
  colors: {
    blue: mantineBlue,
  },

  /** Font family - matching the MUI theme */
  fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif',
  fontFamilyMonospace: 'ui-monospace, SFMono-Regular, "SF Mono", Monaco, Consolas, "Liberation Mono", "Courier New", monospace',

  /** Font sizes - consistent with MUI theme */
  fontSizes: {
    xs: '0.75rem',   // 12px
    sm: '0.875rem',  // 14px - body text
    md: '1rem',      // 16px
    lg: '1.125rem',  // 18px
    xl: '1.25rem',   // 20px
  },

  /** Headings - matching MUI theme typography */
  headings: {
    fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif',
    fontWeight: '600',
    sizes: {
      h1: { fontSize: '2.125rem', lineHeight: '1.15', fontWeight: '700' },
      h2: { fontSize: '1.625rem', lineHeight: '1.2', fontWeight: '700' },
      h3: { fontSize: '1.375rem', lineHeight: '1.25', fontWeight: '600' },
      h4: { fontSize: '1.125rem', lineHeight: '1.3', fontWeight: '600' },
      h5: { fontSize: '1rem', lineHeight: '1.4', fontWeight: '600' },
      h6: { fontSize: '0.875rem', lineHeight: '1.45', fontWeight: '600' },
    },
  },

  /** Spacing - matching our Mantine spacing scale */
  spacing: {
    xs: '10px',
    sm: '12px',
    md: '16px',
    lg: '20px',
    xl: '24px',
  },

  /** Border radius - matching getMantineStyledTheme.ts */
  radius: {
    xs: '2px',
    sm: '4px',
    md: '8px',
    lg: '16px',
    xl: '32px',
  },

  /** Default radius for components */
  defaultRadius: 'sm',

  /** Shadows - subtle shadows matching our MUI theme */
  shadows: {
    xs: '0 1px 3px rgba(0, 0, 0, 0.05), 0 1px 2px rgba(0, 0, 0, 0.1)',
    sm: '0 1px 3px rgba(0, 0, 0, 0.05), 0 1px 2px rgba(0, 0, 0, 0.1)',
    md: '0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06)',
    lg: '0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05)',
    xl: '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)',
  },

  /** Other theme properties */
  focusRing: 'auto',
  respectReducedMotion: true,
  cursorType: 'default',

  /** Component-specific overrides */
  components: {
    Button: {
      defaultProps: {
        size: 'sm',
      },
      styles: {
        root: {
          fontWeight: 500,
          fontSize: '0.875rem',
          minHeight: '36px',
          '&:hover': {
            transform: 'none',
          },
        },
      },
    },
    
    TextInput: {
      styles: {
        input: {
          fontSize: '0.875rem',
        },
        label: {
          fontSize: '0.875rem',
          fontWeight: 500,
        },
      },
    },
    
    Paper: {
      styles: {
        root: {
          border: '1px solid var(--mantine-color-gray-3)',
        },
      },
    },
    
    Modal: {
      styles: {
        title: {
          fontSize: '1.125rem',
          fontWeight: 600,
        },
        body: {
          fontSize: '0.875rem',
        },
      },
    },
    
    Chip: {
      defaultProps: {
        size: 'sm',
      },
      styles: {
        root: {
          fontSize: '0.75rem',
          fontWeight: 500,
          height: '24px',
        },
      },
    },
    
    Tabs: {
      styles: {
        tab: {
          fontSize: '0.875rem',
          fontWeight: 500,
          minHeight: '40px',
        },
      },
    },
  },
});

export default mantineTheme; 