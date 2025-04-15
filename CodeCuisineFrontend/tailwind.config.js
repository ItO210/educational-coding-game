/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}"
  ],
  theme: {
    extend: {
      colors: {
        'aulify-black': '#1D1934',
        'aulify-red' : '#D44D56',
        'aulify-yellow' : '#F6BA27',
        'aulify-blue' : '#0053B1',
        'aulify-dark-blue' : '#005D97',
        'aulify-light-blue' : '#52BEDA'
      },
    },
  },
  plugins: [],
}

