// vite.config.js
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  base: '/PBI24-SEM2/', // 👈 this must match your repo name
  plugins: [react()],
})
