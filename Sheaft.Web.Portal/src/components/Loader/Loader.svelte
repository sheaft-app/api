<script lang="ts">
  import Fa from "svelte-fa";
  import { faCircleNotch } from "@fortawesome/free-solid-svg-icons";

  export let message: string = "Chargement en cours";
</script>

<div class="grid place-items-center h-96">
  <div class="grid place-items-center">
    <Fa icon="{faCircleNotch}" size="5x" spin class="text-default-100" />
    <p class="mt-4">{message}</p>
  </div>
</div>
