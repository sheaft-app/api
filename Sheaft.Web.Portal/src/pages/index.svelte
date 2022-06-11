<script lang="ts">
  import logo from "$assets/svelte.png";
  import { page } from "@roxi/routify";
</script>

<!-- routify:options public=true -->
<!-- routify:options index=1 -->
<!-- routify:options menu="Accueil" -->
<!-- routify:options title="Bienvenue sur votre dashboard" -->
<!-- routify:options icon="fas#desktopAlt" -->

<svelte:head>
  <title>{$page.title}</title>
</svelte:head>

<div class="text-center">
  <img data-ujs-name="Welcome" />
  <h1>Hello Typescript!</h1>
  <p>
    Visit <a href="https://svelte.dev">svelte.dev</a> to learn how to build Svelte apps.
  </p>

  <p>
    Check out <a href="https://github.com/sveltejs/kit#readme">SvelteKit</a> for the officially
    supported framework, also powered by Vite!
  </p>
</div>

<style>
  img {
    height: 16rem;
    width: 16rem;
    margin: auto;
  }

  p {
    max-width: 14rem;
    margin: 1rem auto;
    line-height: 1.35;
  }

  @media (min-width: 480px) {
    h1 {
      max-width: none;
    }

    p {
      max-width: none;
    }
  }
</style>
