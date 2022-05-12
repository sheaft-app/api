<script lang="ts">
  import { onMount } from "svelte";
  import { Router } from "@roxi/routify";
  import { routes } from "$routify/routes";
  import { configureAxios } from "$utils/api";

  onMount(() => {
    configureAxios();
  });
</script>

<Router routes="{routes}" />
