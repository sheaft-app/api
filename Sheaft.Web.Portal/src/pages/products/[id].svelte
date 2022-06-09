<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import Text from "$components/Inputs/Text.svelte";
  import LongText from "$components/Inputs/LongText.svelte";
  import Price from "$components/Inputs/Price.svelte";
  import Vat from "$components/Inputs/Vat.svelte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import Select from '$components/Inputs/Select.svelte'
  import Checkbox from '$components/Inputs/Checkbox.svelte'
  import { onMount } from 'svelte'
  import { mediator } from '$services/mediator'
  import Button from '$components/Buttons/Button.svelte'
  import { createForm } from 'felte'
  import { calculateOnSalePrice } from '$utils/price'
  import { getProductModule } from '$pages/products/module'
  import { ListReturnablesQuery } from '$queries/returnables/listReturnables'
  import { UpdateProductRequest } from '$commands/products/updateProduct'
  import type { Components } from '$types/api'
  import { GetProductQuery } from '$queries/products/getProduct'
  import { currency } from '$utils/format'

  export let id="";
  const module = getProductModule($goto)

  let isLoading = false;
  let isReturnable = false;
  let returnablesOptions: {label:string,value:string}[] = [];

  const { form, data, isSubmitting, isValid, setData } = createForm<Components.Schemas.UpdateProductRequest>({
    onSubmit: async (values) => {
      return await mediator.send(new UpdateProductRequest(
        id, values.name, values.unitPrice, values.vat, values.code, values.description, values.returnableId))
    },
    onSuccess: () => {
      module.goToList()
    },
    onError: (result) => {
    }
  })

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat)

  onMount(async () => {
    try {
      isLoading = true;
      
      const product = await mediator.send(new GetProductQuery(id));
      isReturnable = !!product.returnableId
      setData(product);
      
      const results = await mediator.send(new ListReturnablesQuery(1, 1000));
      returnablesOptions = results.map(r => {
        return { label: r.name ? `${r.name} - ${currency(r.unitPrice)} HT` : '', value: r.id ?? '' }
      }) ?? [];

      isLoading = false;
    }
    catch(exc){
      module.goToList();
    }
  })
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details du produit" -->
<!-- routify:options roles=[] -->

<svelte:head>
  <title>{$page.title}</title>
</svelte:head>

<h1>{$page.title}</h1>

<form use:form>
  <Text
    id='code'
    label="Code"
    bind:value="{$data.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre produit (sera autogénéré si non renseigné)"
    isLoading="{$isSubmitting}"
  />
  <Text
    id='name'
    label="Nom"
    bind:value="{$data.name}"
    placeholder="Le nom de votre produit"
    isLoading="{$isSubmitting}"
  />
  <Price
    id='unitPrice'
    label="Prix HT"
    bind:value="{$data.unitPrice}"
    placeholder="Prix HT de votre produit en €"
    isLoading="{$isSubmitting}"
  />
  <Vat
    id='vat'
    label="TVA"
    bind:value="{$data.vat}"
    isLoading="{$isSubmitting}" />
  <Price
    label="Prix TTC (calculé)"
    value="{onSalePrice}"
    disabled="{true}"
    isLoading="{$isSubmitting}"
    required="{false}"
  />
  <LongText
    id='description'
    label="Description"
    bind:value="{$data.description}"
    required="{false}"
    placeholder="Les ingrédients, la méthode de préparation, tout ce que vous pouvez juger utile de préciser"
    isLoading="{$isSubmitting}"
  />
  <Checkbox
    label="Ce produit est consigné"
    isLoading='{$isSubmitting}'
    bind:value={isReturnable}
    class='mt-3 mb-6'
    required='{false}'/>
  {#if isReturnable}
    <Select
      id='returnableId'
      label="Consigne"
      options='{returnablesOptions}'
      isLoading='{$isSubmitting}'
      disabled='{isLoading}'
      bind:value={$data.returnableId} />
  {/if}
  <FormFooter>
    <Button
      type='button'
      disabled='{$isSubmitting}'
      class='back w-full mx-8'
      on:click='{module.goToList}'
    >Revenir à la liste
    </Button>
    <Button
      type='submit'
      disabled='{!$isValid || $isSubmitting}'
      isLoading='{$isSubmitting}'
      class='accent w-full mx-8'
    >Sauvegarder
    </Button>
  </FormFooter>
</form>
