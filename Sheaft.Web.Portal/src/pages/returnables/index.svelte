<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { format, currency, percent } from '$utils/format'
  import { getReturnableModule } from '$pages/returnables/module'
  import { mediator } from '$services/mediator'
  import { ListReturnablesQuery } from '$queries/returnables/listReturnables'
  import type { Components } from '$types/api'
  import PageContent from '$components/Page/PageContent.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { formatDate, formatDateDistance } from '$utils/date'

  export let pageNumber: number = 1,
    take: number = 10
  const module = getReturnableModule($goto)

  let isLoading = true
  let returnables: Components.Schemas.ReturnableDto[] = []

  onMount(async () => {
    try {
      isLoading = true
      returnables = await mediator.send<ListReturnablesQuery>(
        new ListReturnablesQuery(pageNumber, take)
      )
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

<!-- routify:options menu="Mes consignes" -->
<!-- routify:options title="Mes consignes" -->
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
      {#each returnables as returnable}
        <tr on:click='{() => module.goToDetails(returnable.id)}'>
          <th>{returnable.name}</th>
          <td use:format='{currency}'>{returnable.unitPrice}</td>
          <td use:format='{percent}'>{returnable.vat}</td>
          <td use:format={formatDateDistance}>{returnable.updatedOn}</td>
        </tr>
      {/each}
      {#if returnables?.length < 1}
        <tr>
          <td colspan='3' class='text-center'>Aucune consignes disponible <a href='/returnables/create'>créez votre première consigne</a></td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>
</PageContent>
