<script lang="ts">
  import { page } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { update, getReturnable, returnable } from '$pages/returnables/service'
  import Form from '$components/Form/Form.svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Price from '$components/Inputs/Price.svelte'
  import Vat from '$components/Inputs/Vat.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import { goToList } from '$pages/returnables/router'
  import { createForm } from 'felte'
  import { Paths } from '$types/api'
  import { calculateOnSalePrice } from '$utils/price'

  export let id;
  let isLoading = true;
  
  onMount(async () => {
    isLoading = true;
    await getReturnable(id);    
    isLoading = false;
  });

  const { form, data, isSubmitting, isValid } = createForm<Paths.CreateReturnable.RequestBody>({
    onSubmit: async (values) => {
      const { success, data } = await update(id, values);
      if(success)
        goToList();

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
  $: disabled = isLoading || $isSubmitting;
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
    bind:value="{$returnable.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre produit (autogénéré si non renseigné)"
    isLoading="{$disabled}"
  />
  <Text
    label="Nom"
    bind:value="{$returnable.name}"
    placeholder="Le nom de votre produit"
    isLoading="{$disabled}"
  />
  <Price
    label="Prix HT"
    bind:value="{$returnable.unitPrice}"
    placeholder="Prix HT de votre produit en €"
    isLoading="{$disabled}"
  />
  <Vat label="TVA" bind:value="{$returnable.vat}" isLoading="{$disabled}"  rates='{[0, 0.055, 0.10, 0.20]}'/>
  <Price
    label="Prix TTC (calculé)"
    value="{onSalePrice}"
    disabled="{true}"
    isLoading="{$disabled}"
    required="{false}"
  />
  <FormFooter
    submit="{update}"
    submitText="Mettre à jour"
    cancel="{goToList}"
    disabled="{!isValid}"
    isLoading="{$disabled}"
  />
</Form>
