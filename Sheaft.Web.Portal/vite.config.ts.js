var __require = /* @__PURE__ */ ((x) => typeof require !== "undefined" ? require : typeof Proxy !== "undefined" ? new Proxy(x, {
  get: (a, b) => (typeof require !== "undefined" ? require : a)[b]
}) : x)(function(x) {
  if (typeof require !== "undefined")
    return require.apply(this, arguments);
  throw new Error('Dynamic require of "' + x + '" is not supported');
});

// postcss.config.js
import tailwind from "tailwindcss";

// tailwind.config.js
var colors = __require("tailwindcss/colors.js");
module.exports = {
  plugins: [],
  theme: {
    fontFamily: {
      sans: ["Segoe UI", "sans-serif"]
    },
    extend: {
      colors: {
        primary: colors.violet,
        accent: colors.teal,
        back: colors.gray
      },
      spacing: {
        "128": "32rem",
        "144": "36rem"
      },
      borderRadius: {
        "4xl": "2rem"
      }
    }
  },
  content: ["./index.html", "./src/**/*.{svelte,js,ts}"],
  variants: {
    extend: {}
  },
  darkMode: "media"
};
var tailwind_config_default = module.exports;

// postcss.config.js
import autoprefixer from "autoprefixer";
var postcss_config_default = {
  plugins: [tailwind(tailwind_config_default), autoprefixer]
};

// vite.config.ts
import { svelte } from "@sveltejs/vite-plugin-svelte";
import { defineConfig } from "vite";
import { fileURLToPath, URL } from "url";
import { nodeResolve } from "@rollup/plugin-node-resolve";
import { NodeGlobalsPolyfillPlugin } from "@esbuild-plugins/node-globals-polyfill";
import { NodeModulesPolyfillPlugin } from "@esbuild-plugins/node-modules-polyfill";
import rollupNodePolyFill from "rollup-plugin-node-polyfills";
import vitePluginRequire from "vite-plugin-require";
var production = process.env.NODE_ENV === "production";
var vite_config_default = defineConfig({
  clearScreen: false,
  build: {
    outDir: "./wwwroot",
    rollupOptions: {
      plugins: [
        rollupNodePolyFill()
      ]
    }
  },
  plugins: [
    vitePluginRequire(),
    svelte(),
    nodeResolve()
  ],
  resolve: {
    alias: {
      $routify: fileURLToPath(new URL("./.routify", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $pages: fileURLToPath(new URL("./src/pages", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $components: fileURLToPath(new URL("./src/components", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $utils: fileURLToPath(new URL("./src/utils", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $assets: fileURLToPath(new URL("./src/assets", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $stores: fileURLToPath(new URL("./src/stores", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $styles: fileURLToPath(new URL("./src/styles", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $configs: fileURLToPath(new URL("./src/configs", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $settings: fileURLToPath(new URL("./src/settings", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $enums: fileURLToPath(new URL("./src/enums", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      $types: fileURLToPath(new URL("./src/types", "file:///D:/Projects/Sheaft/api/Sheaft.Web.Portal/vite.config.ts")),
      util: "rollup-plugin-node-polyfills/polyfills/util",
      sys: "util",
      events: "rollup-plugin-node-polyfills/polyfills/events",
      stream: "rollup-plugin-node-polyfills/polyfills/stream",
      path: "rollup-plugin-node-polyfills/polyfills/path",
      querystring: "rollup-plugin-node-polyfills/polyfills/qs",
      punycode: "rollup-plugin-node-polyfills/polyfills/punycode",
      url: "rollup-plugin-node-polyfills/polyfills/url",
      string_decoder: "rollup-plugin-node-polyfills/polyfills/string-decoder",
      http: "rollup-plugin-node-polyfills/polyfills/http",
      https: "rollup-plugin-node-polyfills/polyfills/http",
      os: "rollup-plugin-node-polyfills/polyfills/os",
      assert: "rollup-plugin-node-polyfills/polyfills/assert",
      constants: "rollup-plugin-node-polyfills/polyfills/constants",
      _stream_duplex: "rollup-plugin-node-polyfills/polyfills/readable-stream/duplex",
      _stream_passthrough: "rollup-plugin-node-polyfills/polyfills/readable-stream/passthrough",
      _stream_readable: "rollup-plugin-node-polyfills/polyfills/readable-stream/readable",
      _stream_writable: "rollup-plugin-node-polyfills/polyfills/readable-stream/writable",
      _stream_transform: "rollup-plugin-node-polyfills/polyfills/readable-stream/transform",
      timers: "rollup-plugin-node-polyfills/polyfills/timers",
      console: "rollup-plugin-node-polyfills/polyfills/console",
      vm: "rollup-plugin-node-polyfills/polyfills/vm",
      zlib: "rollup-plugin-node-polyfills/polyfills/zlib",
      tty: "rollup-plugin-node-polyfills/polyfills/tty",
      domain: "rollup-plugin-node-polyfills/polyfills/domain"
    }
  },
  css: {
    postcss: postcss_config_default
  },
  optimizeDeps: {
    esbuildOptions: {
      define: {
        global: "globalThis"
      },
      plugins: [
        NodeGlobalsPolyfillPlugin({
          process: true,
          buffer: true
        }),
        NodeModulesPolyfillPlugin()
      ]
    }
  }
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsicG9zdGNzcy5jb25maWcuanMiLCAidGFpbHdpbmQuY29uZmlnLmpzIiwgInZpdGUuY29uZmlnLnRzIl0sCiAgInNvdXJjZXNDb250ZW50IjogWyJpbXBvcnQgdGFpbHdpbmQgZnJvbSBcInRhaWx3aW5kY3NzXCI7XG5pbXBvcnQgdGFpbHdpbmRDb25maWcgZnJvbSBcIi4vdGFpbHdpbmQuY29uZmlnLmpzXCI7XG5pbXBvcnQgYXV0b3ByZWZpeGVyIGZyb20gXCJhdXRvcHJlZml4ZXJcIjtcblxuZXhwb3J0IGRlZmF1bHQge1xuICBwbHVnaW5zOiBbdGFpbHdpbmQodGFpbHdpbmRDb25maWcpLCBhdXRvcHJlZml4ZXJdLFxufTtcbiIsICJjb25zdCBjb2xvcnMgPSByZXF1aXJlKCd0YWlsd2luZGNzcy9jb2xvcnMuanMnKTtcblxubW9kdWxlLmV4cG9ydHMgPSB7XG4gIHBsdWdpbnM6IFtdLFxuICB0aGVtZToge1xuICAgIGZvbnRGYW1pbHk6IHtcbiAgICAgIHNhbnM6IFtcIlNlZ29lIFVJXCIsICdzYW5zLXNlcmlmJ10sXG4gICAgfSxcbiAgICBleHRlbmQ6IHtcbiAgICAgIGNvbG9yczp7XG4gICAgICAgIHByaW1hcnk6IGNvbG9ycy52aW9sZXQsXG4gICAgICAgIGFjY2VudDogY29sb3JzLnRlYWwsXG4gICAgICAgIGJhY2s6IGNvbG9ycy5ncmF5LFxuICAgICAgfSxcbiAgICAgIHNwYWNpbmc6IHtcbiAgICAgICAgJzEyOCc6ICczMnJlbScsXG4gICAgICAgICcxNDQnOiAnMzZyZW0nLFxuICAgICAgfSxcbiAgICAgIGJvcmRlclJhZGl1czoge1xuICAgICAgICAnNHhsJzogJzJyZW0nLFxuICAgICAgfVxuICAgIH1cbiAgfSxcbiAgY29udGVudDogW1wiLi9pbmRleC5odG1sXCIsIFwiLi9zcmMvKiovKi57c3ZlbHRlLGpzLHRzfVwiXSxcbiAgdmFyaWFudHM6IHtcbiAgICBleHRlbmQ6IHt9LFxuICB9LFxuICBkYXJrTW9kZTogXCJtZWRpYVwiLFxufTtcblxuZXhwb3J0IGRlZmF1bHQgbW9kdWxlLmV4cG9ydHNcbiIsICJpbXBvcnQgcG9zdGNzcyBmcm9tIFwiLi9wb3N0Y3NzLmNvbmZpZy5qc1wiO1xuaW1wb3J0IHsgc3ZlbHRlIH0gZnJvbSBcIkBzdmVsdGVqcy92aXRlLXBsdWdpbi1zdmVsdGVcIjtcbmltcG9ydCB7IGRlZmluZUNvbmZpZyB9IGZyb20gXCJ2aXRlXCI7XG5pbXBvcnQgeyBmaWxlVVJMVG9QYXRoLCBVUkwgfSBmcm9tIFwidXJsXCI7XG5pbXBvcnQgeyBub2RlUmVzb2x2ZSB9IGZyb20gJ0Byb2xsdXAvcGx1Z2luLW5vZGUtcmVzb2x2ZSc7XG5pbXBvcnQgeyBOb2RlR2xvYmFsc1BvbHlmaWxsUGx1Z2luIH0gZnJvbSAnQGVzYnVpbGQtcGx1Z2lucy9ub2RlLWdsb2JhbHMtcG9seWZpbGwnXG5pbXBvcnQgeyBOb2RlTW9kdWxlc1BvbHlmaWxsUGx1Z2luIH0gZnJvbSAnQGVzYnVpbGQtcGx1Z2lucy9ub2RlLW1vZHVsZXMtcG9seWZpbGwnXG5pbXBvcnQgcm9sbHVwTm9kZVBvbHlGaWxsIGZyb20gJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMnXG5pbXBvcnQgdml0ZVBsdWdpblJlcXVpcmUgZnJvbSBcInZpdGUtcGx1Z2luLXJlcXVpcmVcIjtcblxuY29uc3QgcHJvZHVjdGlvbiA9IHByb2Nlc3MuZW52Lk5PREVfRU5WID09PSAncHJvZHVjdGlvbidcblxuZXhwb3J0IGRlZmF1bHQgZGVmaW5lQ29uZmlnKHtcbiAgY2xlYXJTY3JlZW46IGZhbHNlLFxuICBidWlsZDoge1xuICAgIG91dERpcjogXCIuL3d3d3Jvb3RcIixcbiAgICByb2xsdXBPcHRpb25zOiB7XG4gICAgICBwbHVnaW5zOiBbXG4gICAgICAgIC8vIEVuYWJsZSByb2xsdXAgcG9seWZpbGxzIHBsdWdpblxuICAgICAgICAvLyB1c2VkIGR1cmluZyBwcm9kdWN0aW9uIGJ1bmRsaW5nXG4gICAgICAgIHJvbGx1cE5vZGVQb2x5RmlsbCgpXG4gICAgICBdXG4gICAgfVxuICB9LFxuICBwbHVnaW5zOiBbXG4gICAgdml0ZVBsdWdpblJlcXVpcmUoKSxcbiAgICBzdmVsdGUoKSxcbiAgICBub2RlUmVzb2x2ZSgpXG4gIF0sXG4gIHJlc29sdmU6IHtcbiAgICBhbGlhczoge1xuICAgICAgJHJvdXRpZnk6IGZpbGVVUkxUb1BhdGgobmV3IFVSTChcIi4vLnJvdXRpZnlcIiwgXCJmaWxlOi8vL0Q6L1Byb2plY3RzL1NoZWFmdC9hcGkvU2hlYWZ0LldlYi5Qb3J0YWwvdml0ZS5jb25maWcudHNcIikpLFxuICAgICAgJHBhZ2VzOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoXCIuL3NyYy9wYWdlc1wiLCBcImZpbGU6Ly8vRDovUHJvamVjdHMvU2hlYWZ0L2FwaS9TaGVhZnQuV2ViLlBvcnRhbC92aXRlLmNvbmZpZy50c1wiKSksXG4gICAgICAkY29tcG9uZW50czogZmlsZVVSTFRvUGF0aChuZXcgVVJMKFwiLi9zcmMvY29tcG9uZW50c1wiLCBcImZpbGU6Ly8vRDovUHJvamVjdHMvU2hlYWZ0L2FwaS9TaGVhZnQuV2ViLlBvcnRhbC92aXRlLmNvbmZpZy50c1wiKSksXG4gICAgICAkdXRpbHM6IGZpbGVVUkxUb1BhdGgobmV3IFVSTChcIi4vc3JjL3V0aWxzXCIsIFwiZmlsZTovLy9EOi9Qcm9qZWN0cy9TaGVhZnQvYXBpL1NoZWFmdC5XZWIuUG9ydGFsL3ZpdGUuY29uZmlnLnRzXCIpKSxcbiAgICAgICRhc3NldHM6IGZpbGVVUkxUb1BhdGgobmV3IFVSTChcIi4vc3JjL2Fzc2V0c1wiLCBcImZpbGU6Ly8vRDovUHJvamVjdHMvU2hlYWZ0L2FwaS9TaGVhZnQuV2ViLlBvcnRhbC92aXRlLmNvbmZpZy50c1wiKSksXG4gICAgICAkc3RvcmVzOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoXCIuL3NyYy9zdG9yZXNcIiwgXCJmaWxlOi8vL0Q6L1Byb2plY3RzL1NoZWFmdC9hcGkvU2hlYWZ0LldlYi5Qb3J0YWwvdml0ZS5jb25maWcudHNcIikpLFxuICAgICAgJHN0eWxlczogZmlsZVVSTFRvUGF0aChuZXcgVVJMKFwiLi9zcmMvc3R5bGVzXCIsIFwiZmlsZTovLy9EOi9Qcm9qZWN0cy9TaGVhZnQvYXBpL1NoZWFmdC5XZWIuUG9ydGFsL3ZpdGUuY29uZmlnLnRzXCIpKSxcbiAgICAgICRjb25maWdzOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoXCIuL3NyYy9jb25maWdzXCIsIFwiZmlsZTovLy9EOi9Qcm9qZWN0cy9TaGVhZnQvYXBpL1NoZWFmdC5XZWIuUG9ydGFsL3ZpdGUuY29uZmlnLnRzXCIpKSxcbiAgICAgICRzZXR0aW5nczogZmlsZVVSTFRvUGF0aChuZXcgVVJMKFwiLi9zcmMvc2V0dGluZ3NcIiwgXCJmaWxlOi8vL0Q6L1Byb2plY3RzL1NoZWFmdC9hcGkvU2hlYWZ0LldlYi5Qb3J0YWwvdml0ZS5jb25maWcudHNcIikpLFxuICAgICAgJGVudW1zOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoXCIuL3NyYy9lbnVtc1wiLCBcImZpbGU6Ly8vRDovUHJvamVjdHMvU2hlYWZ0L2FwaS9TaGVhZnQuV2ViLlBvcnRhbC92aXRlLmNvbmZpZy50c1wiKSksXG4gICAgICAkdHlwZXM6IGZpbGVVUkxUb1BhdGgobmV3IFVSTChcIi4vc3JjL3R5cGVzXCIsIFwiZmlsZTovLy9EOi9Qcm9qZWN0cy9TaGVhZnQvYXBpL1NoZWFmdC5XZWIuUG9ydGFsL3ZpdGUuY29uZmlnLnRzXCIpKSxcblxuICAgICAgLy8gVGhpcyBSb2xsdXAgYWxpYXNlcyBhcmUgZXh0cmFjdGVkIGZyb20gQGVzYnVpbGQtcGx1Z2lucy9ub2RlLW1vZHVsZXMtcG9seWZpbGwsXG4gICAgICAvLyBzZWUgaHR0cHM6Ly9naXRodWIuY29tL3JlbW9yc2VzL2VzYnVpbGQtcGx1Z2lucy9ibG9iL21hc3Rlci9ub2RlLW1vZHVsZXMtcG9seWZpbGwvc3JjL3BvbHlmaWxscy50c1xuICAgICAgLy8gcHJvY2VzcyBhbmQgYnVmZmVyIGFyZSBleGNsdWRlZCBiZWNhdXNlIGFscmVhZHkgbWFuYWdlZFxuICAgICAgLy8gYnkgbm9kZS1nbG9iYWxzLXBvbHlmaWxsXG4gICAgICB1dGlsOiAncm9sbHVwLXBsdWdpbi1ub2RlLXBvbHlmaWxscy9wb2x5ZmlsbHMvdXRpbCcsXG4gICAgICBzeXM6ICd1dGlsJyxcbiAgICAgIGV2ZW50czogJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL2V2ZW50cycsXG4gICAgICBzdHJlYW06ICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy9zdHJlYW0nLFxuICAgICAgcGF0aDogJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL3BhdGgnLFxuICAgICAgcXVlcnlzdHJpbmc6ICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy9xcycsXG4gICAgICBwdW55Y29kZTogJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL3B1bnljb2RlJyxcbiAgICAgIHVybDogJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL3VybCcsXG4gICAgICBzdHJpbmdfZGVjb2RlcjpcbiAgICAgICAgJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL3N0cmluZy1kZWNvZGVyJyxcbiAgICAgIGh0dHA6ICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy9odHRwJyxcbiAgICAgIGh0dHBzOiAncm9sbHVwLXBsdWdpbi1ub2RlLXBvbHlmaWxscy9wb2x5ZmlsbHMvaHR0cCcsXG4gICAgICBvczogJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL29zJyxcbiAgICAgIGFzc2VydDogJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL2Fzc2VydCcsXG4gICAgICBjb25zdGFudHM6ICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy9jb25zdGFudHMnLFxuICAgICAgX3N0cmVhbV9kdXBsZXg6XG4gICAgICAgICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy9yZWFkYWJsZS1zdHJlYW0vZHVwbGV4JyxcbiAgICAgIF9zdHJlYW1fcGFzc3Rocm91Z2g6XG4gICAgICAgICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy9yZWFkYWJsZS1zdHJlYW0vcGFzc3Rocm91Z2gnLFxuICAgICAgX3N0cmVhbV9yZWFkYWJsZTpcbiAgICAgICAgJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL3JlYWRhYmxlLXN0cmVhbS9yZWFkYWJsZScsXG4gICAgICBfc3RyZWFtX3dyaXRhYmxlOlxuICAgICAgICAncm9sbHVwLXBsdWdpbi1ub2RlLXBvbHlmaWxscy9wb2x5ZmlsbHMvcmVhZGFibGUtc3RyZWFtL3dyaXRhYmxlJyxcbiAgICAgIF9zdHJlYW1fdHJhbnNmb3JtOlxuICAgICAgICAncm9sbHVwLXBsdWdpbi1ub2RlLXBvbHlmaWxscy9wb2x5ZmlsbHMvcmVhZGFibGUtc3RyZWFtL3RyYW5zZm9ybScsXG4gICAgICB0aW1lcnM6ICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy90aW1lcnMnLFxuICAgICAgY29uc29sZTogJ3JvbGx1cC1wbHVnaW4tbm9kZS1wb2x5ZmlsbHMvcG9seWZpbGxzL2NvbnNvbGUnLFxuICAgICAgdm06ICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy92bScsXG4gICAgICB6bGliOiAncm9sbHVwLXBsdWdpbi1ub2RlLXBvbHlmaWxscy9wb2x5ZmlsbHMvemxpYicsXG4gICAgICB0dHk6ICdyb2xsdXAtcGx1Z2luLW5vZGUtcG9seWZpbGxzL3BvbHlmaWxscy90dHknLFxuICAgICAgZG9tYWluOiAncm9sbHVwLXBsdWdpbi1ub2RlLXBvbHlmaWxscy9wb2x5ZmlsbHMvZG9tYWluJ1xuICAgIH0sXG4gIH0sXG4gIGNzczoge1xuICAgIHBvc3Rjc3MsXG4gIH0sXG4gIG9wdGltaXplRGVwczoge1xuICAgIGVzYnVpbGRPcHRpb25zOiB7XG4gICAgICAvLyBOb2RlLmpzIGdsb2JhbCB0byBicm93c2VyIGdsb2JhbFRoaXNcbiAgICAgIGRlZmluZToge1xuICAgICAgICBnbG9iYWw6ICdnbG9iYWxUaGlzJ1xuICAgICAgfSxcbiAgICAgIC8vIEVuYWJsZSBlc2J1aWxkIHBvbHlmaWxsIHBsdWdpbnNcbiAgICAgIHBsdWdpbnM6IFtcbiAgICAgICAgTm9kZUdsb2JhbHNQb2x5ZmlsbFBsdWdpbih7XG4gICAgICAgICAgcHJvY2VzczogdHJ1ZSxcbiAgICAgICAgICBidWZmZXI6IHRydWVcbiAgICAgICAgfSksXG4gICAgICAgIE5vZGVNb2R1bGVzUG9seWZpbGxQbHVnaW4oKVxuICAgICAgXVxuICAgIH1cbiAgfSxcbn0pO1xuIl0sCiAgIm1hcHBpbmdzIjogIjs7Ozs7Ozs7O0FBQUE7OztBQ0FBLElBQU0sU0FBUyxVQUFRO0FBRXZCLE9BQU8sVUFBVTtBQUFBLEVBQ2YsU0FBUyxDQUFDO0FBQUEsRUFDVixPQUFPO0FBQUEsSUFDTCxZQUFZO0FBQUEsTUFDVixNQUFNLENBQUMsWUFBWSxZQUFZO0FBQUEsSUFDakM7QUFBQSxJQUNBLFFBQVE7QUFBQSxNQUNOLFFBQU87QUFBQSxRQUNMLFNBQVMsT0FBTztBQUFBLFFBQ2hCLFFBQVEsT0FBTztBQUFBLFFBQ2YsTUFBTSxPQUFPO0FBQUEsTUFDZjtBQUFBLE1BQ0EsU0FBUztBQUFBLFFBQ1AsT0FBTztBQUFBLFFBQ1AsT0FBTztBQUFBLE1BQ1Q7QUFBQSxNQUNBLGNBQWM7QUFBQSxRQUNaLE9BQU87QUFBQSxNQUNUO0FBQUEsSUFDRjtBQUFBLEVBQ0Y7QUFBQSxFQUNBLFNBQVMsQ0FBQyxnQkFBZ0IsMkJBQTJCO0FBQUEsRUFDckQsVUFBVTtBQUFBLElBQ1IsUUFBUSxDQUFDO0FBQUEsRUFDWDtBQUFBLEVBQ0EsVUFBVTtBQUNaO0FBRUEsSUFBTywwQkFBUSxPQUFPOzs7QUQ1QnRCO0FBRUEsSUFBTyx5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
