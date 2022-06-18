<script lang="ts">
  import Loader from "$components/Loader/Loader.svelte";

  export let isLoading: boolean = false;
  export let message: string | undefined;
</script>

<div>
  {#if isLoading}
    <Loader message="{message}" />
  {:else}
    <slot />
  {/if}
</div>
