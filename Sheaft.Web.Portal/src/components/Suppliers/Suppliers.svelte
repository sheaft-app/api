<script lang='ts'>
  import type { Components } from '$types/api'
  import { goto } from '@roxi/routify'
  import { formatInnerHtml } from "$actions/format"
  import { dateDistance } from '$utils/dates'
  import { status } from '$components/Agreements/utils'
  import { getSupplierModule } from '$components/Suppliers/module'

  const module = getSupplierModule($goto);
  export let suppliers:Components.Schemas.AgreementDto[];
  export let noResultsMessage:string;
  
</script>

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
    {#each suppliers as supplier}
      <tr on:click="{() => module.goToDetails(supplier.id)}">
        <th>{supplier.supplierName}</th>
        <td use:formatInnerHtml={status}>{supplier.status}</td>
        <td use:formatInnerHtml={dateDistance}>{supplier.updatedOn}</td>
      </tr>
    {/each}
    {#if suppliers?.length < 1}
      <tr>
        <td colspan='3' class='text-center'>{noResultsMessage}</td>
      </tr>
    {/if}
    </tbody>
  </table>
</div>  
