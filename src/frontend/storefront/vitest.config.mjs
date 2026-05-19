import react from "@vitejs/plugin-react";
import { fileURLToPath } from "node:url";
import { defineConfig } from "vitest/config";

const root = fileURLToPath(new URL("./", import.meta.url));

export default defineConfig({
  plugins: [react()],
  test: {
    environment: "jsdom",
    exclude: ["**/node_modules/**", "**/tests/e2e/**"],
    globals: true,
    setupFiles: ["./tests/setup.ts"]
  },
  resolve: {
    alias: [{ find: /^@\//, replacement: `${root}/` }]
  }
});
