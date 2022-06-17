<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { ListActiveAgreementsQuery } from '$features/agreements/queries/listActiveAgreements'
  import { statusStr } from '$features/agreements/utils'
  import {formatInnerHtml} from '$directives/format'
  import { dateDistance } from '$utils/dates'
  import { getSupplierModule } from '$features/suppliers/module'

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getSupplierModule($goto);

  let isLoading = true;
  let agreements: Components.Schemas.AgreementDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      agreements = await mediator.send(new ListActiveAgreementsQuery(pageNumber, take));
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });
  
  const actions = [
    {
      name:'Ajouter',
      disabled:false,
      visible: true,
      color:'primary',
      action: () => module.goToAvailableSuppliers()
    }
  ];
  
</script>

<!-- routify:options menu="Mes fournisseurs" -->
<!-- routify:options title="Mes fournisseurs" -->
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
          <th>{agreement.supplierName}</th>
          <td use:formatInnerHtml={statusStr}>{agreement.status}</td>
          <td use:formatInnerHtml={dateDistance}>{agreement.updatedOn}</td>
        </tr>
      {/each}
      {#if agreements?.length < 1}
        <tr>
          <td colspan='3' class='text-center'>Aucun accord commercial en cours, <a href='/suppliers/search'>ajoutez-en un</a></td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>  
</PageContent>
