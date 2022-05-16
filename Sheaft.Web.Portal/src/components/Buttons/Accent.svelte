<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import "./buttons.scss"
  
  export let className: string = "";

  const dispatch = createEventDispatcher();

  const dispatchClick = e => {
    dispatch("click", e);
  };
</script>

<button
  type='button'
  class="f-button {className} bg-accent-300 hover:bg-accent-400 focus:bg-accent-400 active:bg-accent-400"
  on:click|preventDefault="{dispatchClick}"
>
  <slot />
</button>
