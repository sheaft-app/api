<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageContent from "$components/Page/PageContent.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { dateDistance } from "$utils/dates";
  import { currency } from "$utils/money";
  import { percent } from "$utils/percent";
  import { formatInnerHtml } from "$actions/format";
  import { getOrderModule } from "$components/Orders/module";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { ListOrdersQuery } from "$components/Orders/queries/listOrders";

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getOrderModule($goto);

  let isLoading = true;
  let orders: Components.Schemas.OrderDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      orders = await mediator.send(new ListOrdersQuery(pageNumber, take));
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

<!-- routify:options menu="Mes commandes" -->
<!-- routify:options title="Mes commandes" -->
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
        {#each orders as order}
          <tr on:click="{() => module.goToDetails(order.id)}">
            <th>{order.name}</th>
            <td use:formatInnerHtml="{currency}">{order.unitPrice}</td>
            <td use:formatInnerHtml="{percent}">{order.vat}</td>
            <td use:formatInnerHtml="{dateDistance}">{order.updatedOn}</td>
          </tr>
        {/each}
        {#if orders?.length < 1}
          <tr>
            <td colspan="4" class="text-center">Aucune commande</td>
          </tr>
        {/if}
      </tbody>
    </table>
  </div>
</PageContent>
