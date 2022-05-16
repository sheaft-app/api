import colors from 'tailwindcss/colors.js';

export default {
  plugins: [],
  theme: {
    fontFamily: {
      sans: ["Segoe UI", 'sans-serif'],
    },
    extend: {
      colors:{
        primary: colors.violet,
        accent: colors.teal,
        back: colors.gray,
      },
      spacing: {
        '128': '32rem',
        '144': '36rem',
      },
      borderRadius: {
        '4xl': '2rem',
      }
    }
  },
  content: ["./index.html", "./src/**/*.{svelte,js,ts}"],
  variants: {
    extend: {},
  },
  darkMode: "media",
};
