import sveltePreprocess from "svelte-preprocess";

export default {
  disableDependencyReinclusion: ['@roxi/routify'],
  preprocess: sveltePreprocess(),
};
