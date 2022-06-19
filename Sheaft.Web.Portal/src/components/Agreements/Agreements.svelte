<script lang="ts">
  import type { Components } from "$types/api";
  import { formatInnerHtml } from "$actions/format";
  import { dateDistance } from "$utils/dates";
  import { status } from "$components/Agreements/utils";
  import type { IAgreementModule } from "$components/Agreements/module";

  export let agreements: Components.Schemas.AgreementDto[];
  export let noResultsMessage: string;
  export let module: IAgreementModule;
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
      {#each agreements as agreement}
        <tr on:click="{() => module.goToDetails(agreement.id)}">
          <th>{agreement.targetName}</th>
          <td use:formatInnerHtml="{status}">{agreement.status}</td>
          <td use:formatInnerHtml="{dateDistance}">{agreement.updatedOn}</td>
        </tr>
      {/each}
      {#if agreements?.length < 1}
        <tr>
          <td colspan="3" class="text-center">{noResultsMessage}</td>
        </tr>
      {/if}
    </tbody>
  </table>
</div>
