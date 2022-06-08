<script lang="ts">
  import { goto, page } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { update, getProduct } from '$pages/products/service'
  import Form from '$components/Form/Form.svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Price from '$components/Inputs/Price.svelte'
  import Vat from '$components/Inputs/Vat.svelte'
  import LongText from '$components/Inputs/LongText.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import type { Components } from '$types/api'
  import { round } from '$utils/number'
  import Checkbox from '$components/Inputs/Checkbox.svelte'
  import Select from '$components/Inputs/Select.svelte'
  import { mediator } from '$services/mediator'
  import { ListReturnablesQuery } from '$queries/returnables/listReturnables'

  export let id;
  let isLoading = true;
  let isReturnable = false;
  let returnablesOptions = [];

  let product: Components.Schemas.ProductDto |undefined = {
    unitPrice: 0,
    name: "",
    description: "",
    vat: 0.0550,
  };
  
  onMount(async () => {
    isLoading = true;
    try {
      const result = await getProduct(id);

      const results = await mediator.send(new ListReturnablesQuery(1, 1000));
      returnablesOptions = results.map(r => { return {label:r.name, value: r.id}})

      product = result.data;
      isReturnable = product.returnableId != null;

      isLoading = false;
    }
    catch(exc){
      $goto('/products')
    }
  });

  const cancelUpdate = () => {
    $goto("/products/");
  };

  const updateProduct = async () => {
    isLoading = true;
    try {
      const res = await update(id, product);
      $goto('/products/')
    }
    catch(exc) {
      isLoading = false;
    }
  };

  $: fullPrice = round(product.unitPrice * (1 + product.vat / 100));
  $: if(!isReturnable) product.returnableId = null;
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details du produit" -->
<!-- routify:options roles=[] -->

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
    placeholder="Le code de votre produit (autogénéré si non renseigné)"
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
    submit="{updateProduct}"
    submitText="Mettre à jour"
    cancel="{cancelUpdate}"
    isLoading="{isLoading}"
    class='block'
  />
</Form>
