<script lang='ts'>
  import { page } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { listReturnables, returnables } from '$pages/returnables/service'
  import { format, currency, percent } from '$utils/format'
  import { goToDetails } from '$pages/returnables/router'

  let isLoading = true

  onMount(async () => {
    await listReturnables(1, 10)
    isLoading = false
  })
</script>

<!-- routify:options menu="Mes consignes" -->
<!-- routify:options title="Mes consignes" -->
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
      {#each $returnables as returnable}
        <tr on:click={() => goToDetails(returnable.id)}>
          <th scope='row'>{returnable.name}</th>
          <td use:format={currency}>{returnable.unitPrice}</td>
          <td use:format={percent}>{returnable.vat}</td>
        </tr>
      {/each}
      </tbody>
    </table>
  </div>
{/if}
