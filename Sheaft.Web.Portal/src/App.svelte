<script lang="ts">
  import { onMount } from "svelte";
  import { Router } from "@roxi/routify";
  import { routes } from "$routify/routes";
  import { configureAxios } from "$configs/axios";

  onMount(() => {
    configureAxios();
  });
</script>

<Router routes="{routes}" />
