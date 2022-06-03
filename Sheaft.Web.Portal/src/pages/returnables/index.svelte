<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { listReturnables, returnables } from '$pages/returnables/service'
  import { format, currency, percent } from '$utils/format'
  import Search from '$components/Inputs/Search.svelte'

  let isLoading = true

  const navigate = (id: string) => {
    $goto(`/returnables/${id}`)
  }

  onMount(async () => {
    const result = await listReturnables(1, 10)
    if (!result.success) {

    }

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
    <Search placeholder='Rechercher des consignes'/>
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
      {#each $returnables as returnable}
        <tr on:click={() => navigate(returnable.id)}>
          <th scope='row'>{returnable.name}</th>
          <td use:format={currency}>{returnable.unitPrice}</td>
          <td use:format={percent}>{returnable.vat}</td>
          <td>
            <a href='#' class='font-medium text-blue-600 hover:underline'>Edit</a>
          </td>
        </tr>
      {/each}
      </tbody>
    </table>
  </div>
{/if}
