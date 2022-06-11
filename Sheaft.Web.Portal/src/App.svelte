<script lang="ts">
  import { onMount } from "svelte";
  import { Router } from "@roxi/routify";
  import { routes } from "$routify/routes";
  import { api, configureAxios } from "$configs/axios";
  import { authStore } from "$stores/auth";
  import type { Client } from "$types/api";
  import { registerReturnableModule } from "$pages/returnables/module";
  import { registerProductModule } from "$pages/products/module";
  import { registerAccountModule } from "$pages/account/module";
  import { registerCustomerModule } from '$pages/customers/module'
  import { registerAgreementModule } from '$pages/agreements/module'

  onMount(async () => {
    const client = await api.init<Client>();
    configureAxios(client);

    await authStore.startMonitorUserAccessToken();

    registerAccountModule(client, authStore);
    registerProductModule(client);
    registerReturnableModule(client);
    registerAgreementModule(client);
    registerCustomerModule(client);
  });
</script>

<Router routes="{routes}" />
