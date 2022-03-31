import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";


// https://vitejs.dev/config/
export default defineConfig(({ command, mode }) => {

  const env = loadEnv(mode, process.cwd());

  const app = env.BACKEND_CONNECTION ?? "http://localhost:5230";

  return {
    plugins: [react()],
    server: {
      proxy: {
        "/api": app
      },
    },
  };
});
