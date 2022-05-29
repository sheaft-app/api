<script lang="ts">
  import { onMount } from "svelte";
  import { Router } from "@roxi/routify";
  import { routes } from "$routify/routes";
  import { api, configureAxios } from '$configs/axios'
  import { initAuthStore } from "$stores/auth";
  import type { Client } from '$types/api'

  onMount(async () => {
    const client = await api.init<Client>();
    configureAxios(client);
    await initAuthStore();
  });
</script>

<Router routes="{routes}" />
