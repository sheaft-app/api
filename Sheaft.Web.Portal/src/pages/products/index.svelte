<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { listProducts, products } from '$pages/products/service'
  import { format, currency, percent } from '$utils/format'
  import Search from '$components/Inputs/Search.svelte'

  let isLoading = true

  const navigate = (id: string) => {
    $goto(`/products/${id}`)
  }

  onMount(async () => {
    const result = await listProducts(1, 10)
    if (!result.success) {

    }

    isLoading = false
  })
</script>

<!-- routify:options menu="Mes produits" -->
<!-- routify:options title="Mes produits" -->
<!-- routify:options roles=[] -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<h1>{$page.title}</h1>

{#if isLoading}
  <p>Loading</p>
{:else}
  <div class='relative overflow-x-auto shadow-md sm:rounded-lg'>
    <Search placeholder='Rechercher des produits'/>
    <table>
      <thead>
      <tr>
        <th>Nom</th>
        <th>Prix HT</th>
        <th>TVA</th>
        <th></th>
      </tr>
      </thead>
      <tbody>
      {#each $products as product}
        <tr on:click={() => navigate(product.id)}>
          <th scope='row'>{product.name}</th>
          <td use:format={currency}>{product.unitPrice}</td>
          <td use:format={percent}>{product.vat}</td>
          <td>
            <a href='#' class='font-medium text-blue-600 hover:underline'>Edit</a>
          </td>
        </tr>
      {/each}
      </tbody>
    </table>
  </div>
{/if}
