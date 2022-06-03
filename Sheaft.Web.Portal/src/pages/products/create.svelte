<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import Text from "$components/Inputs/Text.svelte";
  import LongText from "$components/Inputs/LongText.svelte";
  import Price from "$components/Inputs/Price.svelte";
  import Vat from "$components/Inputs/Vat.svelte";
  import { round } from "$utils/number";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Form from "$components/Form/Form.svelte";
  import { create } from "./service";
  import type { Paths } from '$types/api'
  import Select from '$components/Inputs/Select.svelte'
  import Checkbox from '$components/Inputs/Checkbox.svelte'
  import { onMount } from 'svelte'
  import { listReturnables } from '$pages/returnables/service'

  let isLoading = false;
  let isReturnable = false;
  let returnablesOptions = [];
  
  const product: Paths.CreateProduct.RequestBody = {
    unitPrice: 0,
    name: "",
    description: "",
    vat: 0.0550,
    returnableId: null
  };

  const cancelCreate = () => {
    $goto("/products/");
  };

  const createProduct = async () => {
    isLoading = true;
    const res = await create(product);
    if (res.success) {
      $goto(`/products/${res.data}`);
      return;
    }

    isLoading = false;
  };
  
  onMount(async () => {
    isLoading = true;
    returnablesOptions = (await listReturnables(1, 1000)).data.items?.map(r => { return {label:r.name, value: r.id}});
    isLoading = false;
  })

  $: fullPrice = round(product.unitPrice * (1 + product.vat / 100));
  $: if(!isReturnable) product.returnableId = null;
</script>

<!-- routify:options menu="Créer" -->
<!-- routify:options index=2 -->
<!-- routify:options title="Ajouter un nouveau produit" -->
<!-- routify:options icon="fas#coffee" -->

<svelte:head>
  <title>{$page.title}</title>
</svelte:head>

<h1>{$page.title}</h1>

<Form class="mt-4 ">
  <Text
    label="Code"
    bind:value="{product.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre produit (sera autogénéré si non renseigné)"
    isLoading="{isLoading}"
  />
  <Text
    label="Nom"
    bind:value="{product.name}"
    placeholder="Le nom de votre produit"
    isLoading="{isLoading}"
  />
  <Price
    label="Prix HT"
    bind:value="{product.unitPrice}"
    placeholder="Prix HT de votre produit en €"
    isLoading="{isLoading}"
  />
  <Vat label="TVA" bind:value="{product.vat}" isLoading="{isLoading}" />
  <Price
    label="Prix TTC (calculé)"
    value="{fullPrice}"
    disabled="{true}"
    isLoading="{isLoading}"
    required="{false}"
  />
  <LongText
    label="Description"
    bind:value="{product.description}"
    required="{false}"
    placeholder="Les ingrédients, la méthode de préparation, tout ce que vous pouvez juger utile de préciser"
    isLoading="{isLoading}"
  />
  <Checkbox label="Ce produit est consigné" {isLoading} bind:value={isReturnable} class='mt-3 mb-6' required='{false}'/>
  {#if isReturnable}
    <Select label="Consigne" options='{returnablesOptions}' {isLoading} bind:value={product.returnableId} />
  {/if}
  <FormFooter
    submit="{createProduct}"
    submitText="Créer"
    cancel="{cancelCreate}"
    isLoading="{isLoading}"
    class='block'
  />
</Form>
