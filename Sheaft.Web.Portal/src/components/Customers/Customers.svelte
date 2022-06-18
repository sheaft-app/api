<script lang='ts'>
  import type { Components } from '$types/api'
  import { goto } from '@roxi/routify'
  import { getCustomerModule } from '$components/Customers/module'
  import { formatInnerHtml } from "$actions/format"
  import { dateDistance } from '$utils/dates'
  import { status } from '$components/Agreements/utils'

  const module = getCustomerModule($goto);
  export let customers:Components.Schemas.AgreementDto[];
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
    {#each customers as customer}
      <tr on:click="{() => module.goToDetails(customer.id)}">
        <th>{customer.customerName}</th>
        <td use:formatInnerHtml={status}>{customer.status}</td>
        <td use:formatInnerHtml={dateDistance}>{customer.updatedOn}</td>
      </tr>
    {/each}
    {#if customers?.length < 1}
      <tr>
        <td colspan='3' class='text-center'>{noResultsMessage}</td>
      </tr>
    {/if}
    </tbody>
  </table>
</div>  
