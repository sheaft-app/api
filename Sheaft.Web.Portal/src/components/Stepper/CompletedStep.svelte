<script lang="ts">
  import { createEventDispatcher } from "svelte";
  import Fa from "svelte-fa";
  import { getFaIconFromFullName } from "$utils/faIcon";

  export let name: string = "";
  export let icon: string = "";

  const dispatch = createEventDispatcher();

  const dispatchClick = (e: any) => {
    dispatch("click", e);
  };
</script>

<div
  class="flex items-center text-primary-600 relative cursor-pointer"
  on:click|preventDefault="{dispatchClick}"
>
  <div
    class="rounded-full transition duration-500 ease-in-out h-12 w-12 py-3 border-2 border-primary-600"
  >
    <Fa icon="{getFaIconFromFullName(icon)}" class="m-auto" />
  </div>
  <div
    class="absolute top-0 -ml-10 text-center mt-16 w-32 text-xs font-medium uppercase text-primary-600"
  >
    {name}
  </div>
</div>
