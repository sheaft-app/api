<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import "./buttons.scss";

  export let className: string = "";

  const dispatch = createEventDispatcher();

  const dispatchClick = (e: any) => {
    dispatch("click", e);
  };
</script>

<button
  type="reset"
  class="f-button {className} bg-warning-300 hover:bg-warning-400 focus:bg-warning-400 active:bg-warning-400"
  on:click|preventDefault="{dispatchClick}"
>
  <slot />
</button>
