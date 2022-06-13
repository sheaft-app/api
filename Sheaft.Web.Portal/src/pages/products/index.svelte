<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { format, currency, percent } from '$utils/format'
  import { mediator } from '$services/mediator'
  import { getProductModule } from '$pages/products/module'
  import { ListProductsQuery } from '$queries/products/listProducts'
  import type { Components } from '$types/api'
  import PageContent from '$components/Page/PageContent.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { formatDate, formatDateDistance } from '$utils/date'

  export let pageNumber: number = 1,
    take: number = 10

  const module = getProductModule($goto)

  let isLoading = true
  let products: Components.Schemas.ProductDto[] = []

  onMount(async () => {
    try {
      isLoading = true
      products = await mediator.send(new ListProductsQuery(pageNumber, take))
      isLoading = false
    } catch (exc) {
      module.goToHome()
    }
  })

  const actions = [
    {
      name:'Ajouter',
      disabled:false,
      color:'primary',
      action: () => module.goToCreate()
    }
  ];
</script>

<!-- routify:options menu="Mes produits" -->
<!-- routify:options title="Mes produits" -->
<!-- routify:options roles=[] -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<PageHeader
  title={$page.title}
  actions={actions}
/>

<PageContent {isLoading}>
  <div class='relative overflow-x-auto shadow-md sm:rounded-lg'>
    <table>
      <thead>
      <tr>
        <th>Nom</th>
        <th>Prix HT</th>
        <th>TVA</th>
        <th>Dernière maj</th>
      </tr>
      </thead>
      <tbody>
      {#each products as product}
        <tr on:click='{() => module.goToDetails(product.id)}'>
          <th>{product.name}</th>
          <td use:format={currency}>{product.unitPrice}</td>
          <td use:format={percent}>{product.vat}</td>
          <td use:format={formatDateDistance}>{product.updatedOn}</td>
        </tr>
      {/each}
      {#if products?.length < 1}
        <tr>
          <td colspan='3' class='text-center'>Aucun produit disponible <a href='/products/create'>créez votre premier produit</a></td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>
</PageContent>
