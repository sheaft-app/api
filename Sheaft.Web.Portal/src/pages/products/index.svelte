<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { listProducts, products } from '$pages/products/service'
  import { format, currency, percent } from '$utils/format'

  let isLoading = true

  const navigate = (id: string) => {
    $goto(`/products/${id}`)
  }

  onMount(async () => {
    const result = await listProducts(1, 10)
    isLoading = false
  })
</script>

<!-- routify:options menu="Mes produits" -->
<!-- routify:options title="Mes produits" -->
<!-- routify:options roles=[] -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<svelte:head>
  <title>{$page.title}</title>
</svelte:head>

<h1>{$page.title}</h1>

{#if isLoading}
  <p>Loading</p>
{:else}
  <div class='relative overflow-x-auto shadow-md sm:rounded-lg'>
    <table>
      <thead>
      <tr>
        <th>Nom</th>
        <th>Prix HT</th>
        <th>TVA</th>
      </tr>
      </thead>
      <tbody>
      {#each $products as product}
        <tr on:click={() => navigate(product.id)}>
          <th scope='row'>{product.name}</th>
          <td use:format={currency}>{product.unitPrice}</td>
          <td use:format={percent}>{product.vat}</td>
        </tr>
      {/each}
      </tbody>
    </table>
  </div>
{/if}
