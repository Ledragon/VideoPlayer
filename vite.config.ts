import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';

// Plugin to remove 'use client' directives from dependencies
const removeUseClientPlugin = () => {
  return {
    name: 'remove-use-client',
    transform(code: string, id: string) {
      if (id.includes('node_modules')) {
        return code.replace(/['"]use client['"];?\s*/g, '');
      }
    }
  };
};

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react(), removeUseClientPlugin()],
  server: {
    port: 2500,
    open: true
  }
});
