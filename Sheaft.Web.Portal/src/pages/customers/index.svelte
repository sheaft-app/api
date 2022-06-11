<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import { mediator } from "$services/mediator";
  import Loader from "$components/Loader/Loader.svelte";
  import { getCustomerModule } from "./module";
  import { ListAgreementsQuery } from "$queries/agreements/listAgreements";
  import type { Components } from "$types/api";

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
</script>

<!-- routify:options menu="Mes clients" -->
<!-- routify:options title="Mes clients" -->
<!-- routify:options roles=[] -->
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
          <th>Status</th>
          <th>Client depuis</th>
          <th>Dernière maj</th>
        </tr>
      </thead>
      <tbody>
        {#each agreements as agreement}
          <tr on:click="{() => module.goToDetails(agreement.id)}">
            <th>{agreement.customer.name}</th>
            <th>{agreement.status}</th>
            <th>{agreement.createdOn}</th>
            <th>{agreement.updatedOn}</th>
          </tr>
        {/each}
      </tbody>
    </table>
  </div>
{/if}
