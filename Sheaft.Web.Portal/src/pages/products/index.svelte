<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import PageContent from "$components/Page/PageContent.svelte";
  import PageHeader from "$components/Page/PageHeader.svelte";
  import { dateDistance } from "$utils/dates";
  import { currency } from "$utils/money";
  import { percent } from "$utils/percent";
  import { formatInnerHtml } from "$actions/format";
  import { getProductModule } from "$components/Products/module";
  import type { Components } from "$types/api";
  import { mediator } from "$components/mediator";
  import { ListProductsQuery } from "$components/Products/queries/listProducts";

  export let pageNumber: number = 1,
    take: number = 10;

  const module = getProductModule($goto);

  let isLoading = true;
  let products: Components.Schemas.ProductDto[] = [];

  onMount(async () => {
    try {
      isLoading = true;
      products = await mediator.send(new ListProductsQuery(pageNumber, take));
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

<!-- routify:options menu="Mes produits" -->
<!-- routify:options title="Mes produits" -->
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
        {#each products as product}
          <tr on:click="{() => module.goToDetails(product.id)}">
            <th>{product.name}</th>
            <td use:formatInnerHtml="{currency}">{product.unitPrice}</td>
            <td use:formatInnerHtml="{percent}">{product.vat}</td>
            <td use:formatInnerHtml="{dateDistance}">{product.updatedOn}</td>
          </tr>
        {/each}
        {#if products?.length < 1}
          <tr>
            <td colspan="4" class="text-center"
              >Aucun produit disponible, <a href="/products/create"
                >créez-en en cliquant ici</a
              ></td>
          </tr>
        {/if}
      </tbody>
    </table>
  </div>
</PageContent>
