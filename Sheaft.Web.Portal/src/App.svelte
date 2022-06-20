<script lang="ts">
  import { Router } from "@roxi/routify";
  import { routes } from "$routify/routes";
  import { onMount } from "svelte";
  import Modal from "svelte-simple-modal";
  import type { Client } from "$types/api";
  import { api, configureAxios } from "./axios";
  import { authStore } from "$components/Account/store";
  import { registerProductModule } from "$components/Products/module";
  import { registerAccountModule } from "$components/Account/module";
  import { registerAgreementModule } from "$components/Agreements/module";
  import { registerCustomerModule } from "$components/Customers/module";
  import { registerSupplierModule } from "$components/Suppliers/module";
  import { registerReturnableModule } from "$components/Returnables/module";
  import { registerOrderModule } from '$components/Orders/module'

  onMount(async () => {
    const client = await api.init<Client>();
    configureAxios(client);

    await authStore.startMonitorUserAccessToken();

    registerProductModule(client, authStore);
    registerReturnableModule(client, authStore);
    registerAccountModule(client, authStore);
    registerAgreementModule(client, authStore);
    registerCustomerModule(client, authStore);
    registerSupplierModule(client, authStore);
    registerOrderModule(client, authStore);
  });
</script>

<Modal>
  <Router routes="{routes}" />
</Modal>
