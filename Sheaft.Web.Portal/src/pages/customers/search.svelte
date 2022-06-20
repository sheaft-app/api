<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import PageContent from "$components/Page/PageContent.svelte";
  import { getCustomerModule } from "$components/Customers/module";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { ListAvailableCustomersQuery } from "$components/Customers/queries/listAvailableCustomers";
  import { address } from "$utils/address";

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getCustomerModule($goto);

  let isLoading = true;
  let customers: Components.Schemas.AvailableCustomerDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      customers = await mediator.send(new ListAvailableCustomersQuery(pageNumber, take));
      isLoading = false;
    } catch (exc) {
      module.goToHome();
    }
  });
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Magasins disponibles" -->

<PageHeader
  title="{$page.title}"
  subtitle="Vous pouvez envoyer une demande d'accord commercial avec les magasins ci-dessous"
  previous="{() => module.goToList()}" />

<PageContent isLoading="{isLoading}">
  <div class="relative overflow-x-auto shadow-md sm:rounded-lg">
    <table>
      <thead>
        <tr>
          <th>Nom</th>
          <th>Adresse de livraison</th>
        </tr>
      </thead>
      <tbody>
        {#each customers as customer}
          <tr on:click="{() => module.goToSearchProfile(customer.id)}">
            <th>{customer.name}</th>
            <th>{@html address(customer.deliveryAddress)}</th>
          </tr>
        {/each}
        {#if customers?.length < 1}
          <tr>
            <td colspan="2" class="text-center">Aucun magasin disponible</td>
          </tr>
        {/if}
      </tbody>
    </table>
  </div>
</PageContent>
