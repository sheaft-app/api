<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import { format, currency, percent } from "$utils/format";
  import { getReturnableModule } from "$pages/returnables/module";
  import { mediator } from "$services/mediator";
  import { ListReturnablesQuery } from "$queries/returnables/listReturnables";
  import Loader from "$components/Loader/Loader.svelte";
  import type { Components } from "$types/api";

  export let pageNumber: number = 1,
    take: number = 10;
  const module = getReturnableModule($goto);

  let isLoading = true;
  let returnables: Components.Schemas.ReturnableDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      returnables = await mediator.send<ListReturnablesQuery>(
        new ListReturnablesQuery(pageNumber, take)
      );
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });
</script>

<!-- routify:options menu="Mes consignes" -->
<!-- routify:options title="Mes consignes" -->
<!-- routify:options index=1 -->
<!-- routify:options default=true -->

<svelte:head>
  <title>{$page.title}</title>
</svelte:head>

<h1>{$page.title}</h1>

{#if isLoading}
  <Loader />
{:else}
  <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
    <table>
      <thead>
        <tr>
          <th>Nom</th>
          <th>Prix HT</th>
          <th>TVA</th>
        </tr>
      </thead>
      <tbody>
        {#each returnables as returnable}
          <tr on:click="{() => module.goToDetails(returnable.id)}">
            <th>{returnable.name}</th>
            <td use:format="{currency}">{returnable.unitPrice}</td>
            <td use:format="{percent}">{returnable.vat}</td>
          </tr>
        {/each}
      </tbody>
    </table>
  </div>
{/if}
