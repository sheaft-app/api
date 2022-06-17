const colors = require('tailwindcss/colors.js');

module.exports = {
  mode: 'jit',
  plugins: [],
  theme: {
    fontFamily: {
      sans: ["Segoe UI", 'sans-serif'],
    },
    extend: {
      colors:{
        primary: colors.violet,
        accent: colors.teal,
        back: colors.stone,
        cancel: colors.neutral,
        danger: colors.red,
        warning: colors.amber,
        info: colors.sky,
        success: colors.emerald,
      },
      spacing: {
        '128': '32rem',
        '144': '36rem',
      },
      borderRadius: {
        '4xl': '2rem',
      },
      height: {
        '96': '24rem',
        '128': '32rem',
      }
    }
  },
  content: ["./index.html", "./src/**/*.{svelte,js,ts}"],
  variants: {
    extend: {},
  },
  darkMode: "media",
};
