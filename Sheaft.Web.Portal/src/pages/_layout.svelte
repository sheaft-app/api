<script lang="ts">
  import { authStore } from '$stores/auth';
  import Login from '$components/Login.svelte';
  const isAuthenticated = authStore.isAuthenticated;
</script>

{#if $isAuthenticated}
  <a href="/">Home</a>
  <a href="/contact">Contact</a>
  <slot />
{:else}
  <Login />
{/if}
