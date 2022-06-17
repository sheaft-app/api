<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { address } from '$utils/addresses'
  import { getSupplierModule } from '$features/suppliers/module'
  import { ListAvailableSuppliersQuery } from '$features/suppliers/queries/listAvailableSuppliers'

  export let pageNumber: number = 1,
    take: number = 10

  const module = getSupplierModule($goto)

  let isLoading = true
  let suppliers: Components.Schemas.AvailableSupplierDto[] = []

  onMount(async () => {
    try {
      isLoading = true
      suppliers = await mediator.send(new ListAvailableSuppliersQuery(pageNumber, take))
      isLoading = false
    } catch (exc) {
      module.goToHome()
    }
  })
</script>

<!-- routify:options index=4 -->
<!-- routify:options title="Producteurs disponibles" -->

<PageHeader
  title={$page.title}
  subtitle="Ce sont les producteurs avec lesquels vous n'avez pas encore d'accord commercial"
  previous='{() => module.goToSuppliers()}' />

<PageContent {isLoading}>
  <div class='relative overflow-x-auto shadow-md sm:rounded-lg'>
    <table>
      <thead>
      <tr>
        <th>Nom</th>
        <th>Adresse d'expedition</th>
      </tr>
      </thead>
      <tbody>
      {#each suppliers as supplier}
        <tr on:click='{() => module.goToSupplier(supplier.id)}'>
          <th>{supplier.name}</th>
          <th>{@html address(supplier.deliveryAddress)}</th>
        </tr>
      {/each}
      {#if suppliers?.length < 1}
        <tr>
          <td colspan='2' class='text-center'>Aucun producteur disponible</td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>
</PageContent>
