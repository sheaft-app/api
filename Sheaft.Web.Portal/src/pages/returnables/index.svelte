<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageContent from "$components/Page/PageContent.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { currency } from "$utils/money";
  import { percent } from "$utils/percent";
  import { dateDistance } from "$utils/dates";
  import { formatInnerHtml } from "$actions/format";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { ListReturnablesQuery } from "$components/Returnables/queries/listReturnables";
  import { getReturnableModule } from "$components/Returnables/module";

  export let pageNumber: number = 1,
    take: number = 10;
  const module = getReturnableModule($goto);

  let isLoading = true;
  let returnables: Components.Schemas.ReturnableDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      returnables = await mediator.send(new ListReturnablesQuery(pageNumber, take));
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

<!-- routify:options menu="Mes consignes" -->
<!-- routify:options title="Mes consignes" -->
<!-- routify:options index=true -->
<!-- routify:options default=true -->

<PageHeader title="{$page.title}" actions="{actions}" />

<PageContent isLoading="{isLoading}">
  <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
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
          <tr on:click="{() => module.goToDetails(returnable.id)}">
            <th>{returnable.name}</th>
            <td use:formatInnerHtml="{currency}">{returnable.unitPrice}</td>
            <td use:formatInnerHtml="{percent}">{returnable.vat}</td>
            <td use:formatInnerHtml="{dateDistance}">{returnable.updatedOn}</td>
          </tr>
        {/each}
        {#if returnables?.length < 1}
          <tr>
            <td colspan="4" class="text-center"
              >Aucune consigne disponible, <a href="/returnables/create"
                >créez-en une en cliquant ici</a
              ></td>
          </tr>
        {/if}
      </tbody>
    </table>
  </div>
</PageContent>
