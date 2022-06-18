<script lang="ts">
  import { page, goto } from "@roxi/routify";
  import { onMount } from "svelte";
  import Button from "$components/Button/Button.svelte";
  import { createForm } from "felte";
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { calculateOnSalePrice } from '$utils/money'
  import { validator } from '@felte/validator-vest'
  import { suite } from '$pages/products/validators'
  import reporterDom from '@felte/reporter-dom';
  import { getProductModule } from '$components/Products/module'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { CreateProductCommand } from '$components/Products/commands/createProduct'
  import { ListReturnablesOptionsQuery } from '$components/Products/queries/listReturnablesOptions'
  import Input from '$components/Input/Input.svelte'
  import Vat from '$components/Vat/Vat.svelte'
  import TextArea from '$components/TextArea/TextArea.svelte'
  import Checkbox from '$components/Checkbox/Checkbox.svelte'
  import Select from '$components/Select/Select.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'

  const module = getProductModule($goto);

  let isLoading = false;
  let isReturnable = false;
  let returnablesOptions: { label: string; value: string }[] = [];
  
  const { form, data, isSubmitting, isValid } =
    createForm<Components.Schemas.CreateProductRequest>({
      initialValues:{
        vat:5.5
      },
      onSubmit: async values => {
        return await mediator.send(
          new CreateProductCommand(
            values.name,
            values.unitPrice,
            values.vat,
            values.code,
            values.description,
            values.returnableId
          )
        );
      },
      onSuccess: (id: string) => {
        module.goToDetails(id);
      },
      extend: [
        <any>validator({ suite }),
        reporterDom({single:true})
      ]
    });

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat);

  onMount(async () => {
    try {
      isLoading = true;
      returnablesOptions = await mediator.send(new ListReturnablesOptionsQuery());
      isLoading = false;
    } catch (exc) {
      module.goToList();
    }
  });
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Ajouter un nouveau produit" -->

<PageHeader
  title={$page.title}
  previous='{() => module.goToList()}'
/>

<form use:form>
  <Input
    id="code"
    label="Code"
    bind:value="{$data.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre produit (sera autogénéré si non renseigné)"
    disabled="{$isSubmitting}"
  />
  <Input
    id="name"
    label="Nom"
    bind:value="{$data.name}"
    placeholder="Le nom de votre produit"
    disabled="{$isSubmitting}"
  />
  <Input
    id="unitPrice"
    label="Prix HT"
    bind:value="{$data.unitPrice}"
    placeholder="Prix HT de votre produit en €"
    disabled="{$isSubmitting}"
  />
  <Vat id="vat" label="TVA" bind:value="{$data.vat}" disabled="{$isSubmitting}" />
  <Input
    type='number'
    label="Prix TTC (calculé)"
    value="{onSalePrice}"
    disabled="{true}"
    required="{false}"
  />
  <TextArea
    id="description"
    label="Description"
    bind:value="{$data.description}"
    required="{false}"
    placeholder="Les ingrédients, la méthode de préparation, tout ce que vous pouvez juger utile de préciser"
    disabled="{$isSubmitting}"
  />
  <Checkbox
    id='hasReturnable'
    label="Ce produit est consigné"
    disabled="{$isSubmitting}"
    bind:value="{isReturnable}"
    class="mt-3 mb-6"
  />
  {#if isReturnable}
    <Select
      id="returnableId"
      label="Consigne"
      options="{returnablesOptions}"
      disabled="{$isSubmitting|| isLoading}"
      bind:value="{$data.returnableId}"
    />
  {/if}
  <FormFooter>
    <Button
      type="button"
      disabled="{$isSubmitting}"
      class="back w-full mx-8"
      on:click="{module.goToList}"
      >Revenir à la liste
    </Button>
    <Button
      type="submit"
      disabled="{$isSubmitting}"
      isLoading="{$isSubmitting}"
      class="accent w-full mx-8"
      >Créer
    </Button>
  </FormFooter>
</form>
