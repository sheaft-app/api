import postcss from "./postcss.config.js";
import { svelte } from "@sveltejs/vite-plugin-svelte";
import { defineConfig } from "vite";
import { fileURLToPath, URL } from "url";

export default defineConfig({
  build: {
    outDir: "./wwwroot",
  },
  plugins: [svelte()],
  resolve: {
    alias: {
      $routes: fileURLToPath(new URL("./src/routes", import.meta.url)),
      $components: fileURLToPath(new URL("./src/components", import.meta.url)),
      $utils: fileURLToPath(new URL("./src/utils", import.meta.url)),
      $assets: fileURLToPath(new URL("./src/assets", import.meta.url)),
      $stores: fileURLToPath(new URL("./src/stores", import.meta.url)),
      $styles: fileURLToPath(new URL("./src/styles", import.meta.url)),
    },
  },
  css: {
    postcss,
  },
});
