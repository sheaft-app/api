<script lang='ts'>
  import { onMount } from 'svelte'
  import { Router } from '@roxi/routify'
  import { routes } from '$routify/routes'
  import Modal from 'svelte-simple-modal'
  import { registerProductModule } from '$features/products/module'
  import { registerAccountModule } from '$features/account/module'
  import { registerAgreementModule } from '$features/agreements/module'
  import { api, configureAxios } from '$features/axios'
  import type { Client } from '$features/api'
  import { authStore } from "$components/Auth/auth";
  import { registerCustomerModule } from '$features/customers/module'
  import { registerSupplierModule } from '$features/suppliers/module'

  onMount(async () => {
    const client = await api.init<Client>()
    configureAxios(client)

    await authStore.startMonitorUserAccessToken()

    registerProductModule(client)
    registerAccountModule(client, authStore)
    registerAgreementModule(client, authStore)
    registerCustomerModule(client, authStore)
    registerSupplierModule(client, authStore)
  })
</script>

<Modal>
  <Router routes='{routes}' />
</Modal>
