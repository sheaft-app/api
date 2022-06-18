<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import type { Components } from '$types/api'
  import {formatInnerHtml} from '$components/Actions/format'
  import { dateDistance } from '$utils/dates'
  import { getSupplierModule } from '$components/Suppliers/module'
  import { mediator } from '$components/mediator'
  import { ListReceivedAgreementsQuery } from '$components/Agreements/queries/listReceivedAgreements'
  import { status } from '$components/Agreements/utils'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getSupplierModule($goto);

  let isLoading = true;
  let agreements: Components.Schemas.AgreementDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      agreements = await mediator.send(new ListReceivedAgreementsQuery(pageNumber, take));
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });
  
</script>

<!-- routify:options menu="Demandes reçues" -->
<!-- routify:options title="Demandes d'accord commercial reçues" -->
<!-- routify:options index=3 -->

<PageHeader
  title={$page.title}
  subtitle="Toutes les demandes en attente de votre acceptation, sont listées ci-dessous"
  previous='{() => module.goToSuppliers()}'
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
          <th>{agreement.supplierName}</th>
          <td use:formatInnerHtml={status}>{agreement.status}</td>
          <td use:formatInnerHtml={dateDistance}>{agreement.updatedOn}</td>
        </tr>
      {/each}
      {#if agreements?.length < 1}
        <tr>
          <td colspan='3' class='text-center'>Aucun accord commercial reçu en attente</td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>  
</PageContent>
