<script lang="ts">
  import { page } from "@roxi/routify";
  import Text from "$components/Inputs/Text.svelte";
  import Price from "$components/Inputs/Price.svelte";
  import Vat from "$components/Inputs/Vat.svelte";
  import FormFooter from "$components/Form/FormFooter.svelte";
  import { create } from './service'
  import { goToDetails, goToList } from './router'
  import { calculateOnSalePrice } from '$utils/price'
  import { createForm } from 'felte';
  import { Paths } from '$types/api';

  const { form, data, isSubmitting, isValid } = createForm<Paths.CreateReturnable.RequestBody>({
    onSubmit: async (values) => {
      const { success, data } = await create(values);
      if(success)
        goToDetails(data);
      
      return data;
    },
    onSuccess: (response)=>{
      console.log(response);
    },
    onError: (error)=>{
      console.log(error);
    }
  })
  
  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat);
</script>

<!-- routify:options menu="Créer" -->
<!-- routify:options index=2 -->
<!-- routify:options title="Ajouter une nouvelle consigne" -->
<!-- routify:options icon="fas#coffee" -->

<svelte:head>
  <title>{$page.title}</title>
</svelte:head>

<h1>{$page.title}</h1>

<form use:form>
  <Text
    label="Code"
    bind:value="{$data.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre consigne (sera autogénéré si non renseigné)"
    isLoading="{$isSubmitting}"
  />
  <Text
    label="Nom"
    bind:value="{$data.name}"
    placeholder="Le nom de votre consigne"
    isLoading="{$isSubmitting}"
  />
  <Price
    label="Prix HT"
    bind:value="{$data.unitPrice}"
    placeholder="Prix HT de votre consigne en €"
    isLoading="{$isSubmitting}"
  />
  <Vat label="TVA" bind:value="{$data.vat}" isLoading="{$isSubmitting}" rates='{[0, 0.055, 0.10, 0.20]}' />
  <Price
    label="Prix TTC (calculé)"
    value="{onSalePrice}"
    disabled="{true}"
    isLoading="{$isSubmitting}"
    required="{false}"
  />
  <FormFooter
    submit="{create}"
    submitText="Créer"
    cancel="{goToList}"
    disabled="{!$isValid}"
    isLoading="{$isSubmitting}"
  />
</Form>
