<script lang="ts">
  import { createEventDispatcher } from 'svelte';
  export let className: string = '';

  const dispatch = createEventDispatcher();

  const dispatchClick = e => {
    dispatch('click', e);
  };
</script>

<button
  type="submit"
  class="f-button f-primary-color {className}"
  on:click|preventDefault="{dispatchClick}"
>
  <slot />
</button>
