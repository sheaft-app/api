<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import "./buttons.scss";

  export let className: string = "";

  const dispatch = createEventDispatcher();

  const dispatchClick = e => {
    dispatch("click", e);
  };
</script>

<button
  type="button"
  class="f-button {className} bg-primary-600 hover:bg-primary-700 focus:bg-primary-700 active:bg-primary-700"
  on:click|preventDefault="{dispatchClick}"
>
  <slot />
</button>
