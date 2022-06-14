<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import { getAgreementModule } from '$features/agreements/module'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { ListAgreementsQuery } from '$features/agreements/queries/listAgreements'
  import { statusStr } from '$features/agreements/utils'
  import {formatInnerHtml} from '$directives/format'
  import { dateDistance } from '$utils/dates'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getAgreementModule($goto);

  let isLoading = true;
  let agreements: Components.Schemas.AgreementDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      agreements = await mediator.send(new ListAgreementsQuery(pageNumber, take));
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });
  
  const actions = [
    {
      name:'Ajouter',
      disabled:false,
      color:'primary',
      action: () => module.goToAvailableCustomers()
    }
  ];
  
</script>

<!-- routify:options menu="Mes clients" -->
<!-- routify:options title="Mes clients" -->
<!-- routify:options roles=[] -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<PageHeader
  title={$page.title}
  actions={actions}
/>
<PageContent {isLoading}>
  <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
    <table>
      <thead>
      <tr>
        <th>Nom</th>
        <th>Statut</th>
        <th>Dernière maj</th>
      </tr>
      </thead>
      <tbody>
      {#each agreements as agreement}
        <tr on:click="{() => module.goToDetails(agreement.id)}">
          <th>{agreement.customerName}</th>
          <td use:formatInnerHtml={statusStr}>{agreement.status}</td>
          <td use:formatInnerHtml={dateDistance}>{agreement.updatedOn}</td>
        </tr>
      {/each}
      </tbody>
    </table>
  </div>  
</PageContent>
