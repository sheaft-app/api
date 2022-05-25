<script lang="ts">
  import { onMount } from "svelte";
  import { Router } from "@roxi/routify";
  import { routes } from "$routify/routes";
  import { configureAxios } from "$configs/axios";
  import { initAuthStore } from "$stores/auth";

  onMount(async () => {
    configureAxios();
    await initAuthStore();
  });
</script>

<Router routes="{routes}" />
