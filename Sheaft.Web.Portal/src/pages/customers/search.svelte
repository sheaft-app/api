<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import { getAgreementModule } from '$features/agreements/module'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { ListAvailableCustomersQuery } from '$features/agreements/queries/listAvailableCustomers'
  import { address } from '$utils/addresses'

  export let pageNumber: number = 1,
    take: number = 10

  const module = getAgreementModule($goto)

  let isLoading = true
  let customers: Components.Schemas.AvailableCustomerDto[] = []

  onMount(async () => {
    try {
      isLoading = true
      customers = await mediator.send(new ListAvailableCustomersQuery(pageNumber, take))
      isLoading = false
    } catch (exc) {
      module.goToHome()
    }
  })
</script>

<!-- routify:options index=1 -->
<!-- routify:options menu="Ajouter" -->
<!-- routify:options title="Magasins disponibles" -->

<PageHeader
  title={$page.title}
  subtitle="Ce sont les magasins avec lesquels vous n'avez pas encore d'accord commercial"
  previous='{() => module.goToCustomers()}' />

<PageContent {isLoading}>
  <div class='relative overflow-x-auto shadow-md sm:rounded-lg'>
    <table>
      <thead>
      <tr>
        <th>Nom</th>
        <th>Adresse de livraison</th>
      </tr>
      </thead>
      <tbody>
      {#each customers as customer}
        <tr on:click='{() => module.goToCustomer(customer.id)}'>
          <th>{customer.name}</th>
          <th>{@html address(customer.deliveryAddress)}</th>
        </tr>
      {/each}
      {#if customers?.length < 1}
        <tr>
          <td colspan='2' class='text-center'>Aucun magasin disponible</td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>
</PageContent>
