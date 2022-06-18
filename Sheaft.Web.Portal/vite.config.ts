import postcss from "./postcss.config.js";
import { svelte } from "@sveltejs/vite-plugin-svelte";
import { defineConfig } from "vite";
import { fileURLToPath, URL } from "url";
import { nodeResolve } from '@rollup/plugin-node-resolve';
import { NodeGlobalsPolyfillPlugin } from '@esbuild-plugins/node-globals-polyfill'
import { NodeModulesPolyfillPlugin } from '@esbuild-plugins/node-modules-polyfill'
import rollupNodePolyFill from 'rollup-plugin-node-polyfills'

const production = process.env.NODE_ENV === 'production'

export default defineConfig({
  clearScreen: false,
  build: {
    outDir: "./wwwroot",
    rollupOptions: {
      plugins: [
        // Enable rollup polyfills plugin
        // used during production bundling
        rollupNodePolyFill(),
      ]
    }
  },
  plugins: [
    svelte(),
    nodeResolve({
      dedupe: ['svelte', 'svelte/transition', 'svelte/internal'], // important!
    }),
  ],
  resolve: {
    alias: {
      $routify: fileURLToPath(new URL("./.routify", import.meta.url)),
      $pages: fileURLToPath(new URL("./src/pages", import.meta.url)),
      $components: fileURLToPath(new URL("./src/components", import.meta.url)),
      $utils: fileURLToPath(new URL("./src/utils", import.meta.url)),
      $assets: fileURLToPath(new URL("./src/assets", import.meta.url)),
      $styles: fileURLToPath(new URL("./src/styles", import.meta.url)),
      $enums: fileURLToPath(new URL("./src/enums", import.meta.url)),
      $types: fileURLToPath(new URL("./src/types", import.meta.url)),

      // This Rollup aliases are extracted from @esbuild-plugins/node-modules-polyfill,
      // see https://github.com/remorses/esbuild-plugins/blob/master/node-modules-polyfill/src/polyfills.ts
      // process and buffer are excluded because already managed
      // by node-globals-polyfill
      util: 'rollup-plugin-node-polyfills/polyfills/util',
      sys: 'util',
      events: 'rollup-plugin-node-polyfills/polyfills/events',
      stream: 'rollup-plugin-node-polyfills/polyfills/stream',
      path: 'rollup-plugin-node-polyfills/polyfills/path',
      querystring: 'rollup-plugin-node-polyfills/polyfills/qs',
      punycode: 'rollup-plugin-node-polyfills/polyfills/punycode',
      url: 'rollup-plugin-node-polyfills/polyfills/url',
      string_decoder:
        'rollup-plugin-node-polyfills/polyfills/string-decoder',
      http: 'rollup-plugin-node-polyfills/polyfills/http',
      https: 'rollup-plugin-node-polyfills/polyfills/http',
      os: 'rollup-plugin-node-polyfills/polyfills/os',
      assert: 'rollup-plugin-node-polyfills/polyfills/assert',
      constants: 'rollup-plugin-node-polyfills/polyfills/constants',
      _stream_duplex:
        'rollup-plugin-node-polyfills/polyfills/readable-stream/duplex',
      _stream_passthrough:
        'rollup-plugin-node-polyfills/polyfills/readable-stream/passthrough',
      _stream_readable:
        'rollup-plugin-node-polyfills/polyfills/readable-stream/readable',
      _stream_writable:
        'rollup-plugin-node-polyfills/polyfills/readable-stream/writable',
      _stream_transform:
        'rollup-plugin-node-polyfills/polyfills/readable-stream/transform',
      timers: 'rollup-plugin-node-polyfills/polyfills/timers',
      console: 'rollup-plugin-node-polyfills/polyfills/console',
      vm: 'rollup-plugin-node-polyfills/polyfills/vm',
      zlib: 'rollup-plugin-node-polyfills/polyfills/zlib',
      tty: 'rollup-plugin-node-polyfills/polyfills/tty',
      domain: 'rollup-plugin-node-polyfills/polyfills/domain'
    },
  },
  css: {
    postcss,
  },
  optimizeDeps: {
    esbuildOptions: {
      // Node.js global to browser globalThis
      define: {
        global: 'globalThis'
      },
      // Enable esbuild polyfill plugins
      plugins: [
        NodeGlobalsPolyfillPlugin({
          process: true,
          buffer: true
        }),
        NodeModulesPolyfillPlugin()
      ]
    }
  },
});
