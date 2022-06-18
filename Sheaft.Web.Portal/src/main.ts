import App from "./App.svelte";
import HMR from "@roxi/routify/hmr";
import "$styles/tailwind.css";
import "$styles/global.scss";
import "$styles/tables.scss";
import "$styles/inputs.scss";
import "$styles/validations.scss";

const app = HMR(App, { target: document.body }, "app");

export default app;
