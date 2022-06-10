<script lang="ts">
  import { onMount } from "svelte";
  import { Router } from '@roxi/routify'
  import { routes } from "$routify/routes";
  import { api, configureAxios } from '$configs/axios'
  import { authStore } from "$stores/auth";
  import type { Client } from '$types/api'
  import { initReturnableModule } from '$pages/returnables/module'
  import { initProductModule } from '$pages/products/module'
  import { initAuthModule } from '$pages/auth/module'

  onMount(async () => {
    const client = await api.init<Client>();
    configureAxios(client);

    await authStore.startMonitorUserAccessToken();
    
    initAuthModule(client, authStore);
    initProductModule(client);
    initReturnableModule(client);
       
  });
</script>

<Router routes="{routes}" />
