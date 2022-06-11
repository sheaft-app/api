<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import { format, currency, percent } from "$utils/format";
  import { mediator } from "$services/mediator";
  import Loader from "$components/Loader/Loader.svelte";
  import { getProductModule } from "$pages/products/module";
  import { ListProductsQuery } from "$queries/products/listProducts";
  import type { Components } from "$types/api";

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
</script>

<!-- routify:options menu="Mes produits" -->
<!-- routify:options title="Mes produits" -->
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
          <th>Prix HT</th>
          <th>TVA</th>
        </tr>
      </thead>
      <tbody>
        {#each products as product}
          <tr on:click="{() => module.goToDetails(product.id)}">
            <th>{product.name}</th>
            <td use:format="{currency}">{product.unitPrice}</td>
            <td use:format="{percent}">{product.vat}</td>
          </tr>
        {/each}
      </tbody>
    </table>
  </div>
{/if}
