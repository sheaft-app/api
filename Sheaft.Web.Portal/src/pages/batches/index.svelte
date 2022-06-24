<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageContent from "$components/Page/PageContent.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { dateDistance, dateStr } from '$utils/dates'
  import { formatInnerHtml } from "$actions/format";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { ListBatchesQuery } from "$components/Batches/queries/listBatches";
  import { getBatchModule } from "$components/Batches/module";
  import { dateKindShort } from '$components/Batches/utils'

  export let pageNumber: number = 1,
    take: number = 10;
  const module = getBatchModule($goto);

  let isLoading = true;
  let batches: Components.Schemas.BatchDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      batches = await mediator.send(new ListBatchesQuery(pageNumber, take));
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });

  const actions = [
    {
      name: "Ajouter",
      disabled: false,
      visible: true,
      color: "primary",
      action: () => module.goToCreate()
    }
  ];
</script>

<!-- routify:options menu="Traçabilité" -->
<!-- routify:options title="Traçabilité" -->
<!-- routify:options index=true -->
<!-- routify:options default=true -->

<PageHeader title="{$page.title}" actions="{actions}" />

<PageContent isLoading="{isLoading}">
  <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
    <table>
      <thead>
        <tr>
          <th>N° de lot</th>
          <th>Type</th>
          <th>Date</th>
          <th>Dernière maj</th>
        </tr>
      </thead>
      <tbody>
        {#each batches as batch}
          <tr on:click="{() => module.goToDetails(batch.id)}">
            <th>{batch.number}</th>
            <td>{dateKindShort(batch.kind)}</td>
            <td>{dateStr(batch.expirationDate)}</td>
            <td use:formatInnerHtml="{dateDistance}">{batch.updatedOn}</td>
          </tr>
        {/each}
        {#if batches?.length < 1}
          <tr>
            <td colspan="4" class="text-center"
              >Aucun numéro de lot disponible, <a href="/batches/create"
                >créez-en un en cliquant ici</a
              ></td>
          </tr>
        {/if}
      </tbody>
    </table>
  </div>
</PageContent>
