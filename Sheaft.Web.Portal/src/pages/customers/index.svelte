<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import { mediator } from "$services/mediator";
  import { getCustomerModule } from "./module";
  import { ListAgreementsQuery } from "$queries/agreements/listAgreements";
  import type { Components } from "$types/api";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import { formatAgreementStatus } from '$pages/agreements/utils'
  import { formatDateDistance } from '$utils/date'
  import { format } from '$utils/format'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getCustomerModule($goto);

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
      action: () => module.goToCustomers()
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
          <td use:format={formatAgreementStatus}>{agreement.status}</td>
          <td use:format={formatDateDistance}>{agreement.updatedOn}</td>
        </tr>
      {/each}
      </tbody>
    </table>
  </div>  
</PageContent>
