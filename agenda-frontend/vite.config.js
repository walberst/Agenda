import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'

export default defineConfig({
  plugins: [vue()],

  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },

  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        changeOrigin: true,
        secure: false,
      },
    },
  },

  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: [
      './src/tests/setup.js',         
      './src/tests/setupApiMock.js'  
    ],
  },
})
