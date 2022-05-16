module.exports = {
  parser: "@typescript-eslint/parser",
  extends: [
    "eslint:recommended",
  ],
  parserOptions: {
    ecmaVersion: 2020,
    sourceType: "module",
    tsconfigRootDir: __dirname,
    project: ["./tsconfig.json"],
    extraFileExtensions: [".svelte", ".cjs"]
  },
  env: {
    es6: true,
    browser: true
  },
  overrides: [
    {
      files: ["*.svelte"],
      processor: "svelte3/svelte3"
    }
  ],
  settings: {
    "svelte3/typescript": true,
    // ignore style tags in Svelte because of Tailwind CSS
    // See https://github.com/sveltejs/eslint-plugin-svelte3/issues/70
    "svelte3/ignore-styles": () => true
  },
  plugins: ["svelte3", "@typescript-eslint"],
  ignorePatterns: ["node_modules"],
  rules: {
    "@typescript-eslint/no-unsafe-assignment": "warning"
  }
}
