<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { format, currency, percent } from '$utils/format'
  import { getReturnableModule } from '$pages/returnables/module'
  import { mediator } from '$services/mediator'
  import { ListReturnablesQuery } from '$queries/returnables/listReturnables'

  let isLoading = true
  const module = getReturnableModule($goto);
  let returnables = [];

  onMount(async () => {
    isLoading = true;
    try {
      returnables = await mediator.send<ListReturnablesQuery>(new ListReturnablesQuery(1, 10))
      isLoading = false
    }
    catch(exc){   
      $goto('/')
    }
  })
</script>

<!-- routify:options menu="Mes consignes" -->
<!-- routify:options title="Mes consignes" -->
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
      {#each returnables as returnable}
        <tr on:click={() => module.goToDetails(returnable.id)}>
          <th scope='row'>{returnable.name}</th>
          <td use:format={currency}>{returnable.unitPrice}</td>
          <td use:format={percent}>{returnable.vat}</td>
        </tr>
      {/each}
      </tbody>
    </table>
  </div>
{/if}
