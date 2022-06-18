<script lang="ts">
  export let title: string = "Accueil";
</script>

<div class="text-center">
  <img data-ujs-name="Welcome" />
  <h1>{title}</h1>
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
