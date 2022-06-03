<script lang="ts">
  import { goto, page } from '@roxi/routify'
  import { onMount } from 'svelte'
  import { update, getReturnable } from '$pages/returnables/service'
  import Form from '$components/Form/Form.svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Price from '$components/Inputs/Price.svelte'
  import Vat from '$components/Inputs/Vat.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import type { Components } from '$types/api'
  import { round } from '$utils/number'

  export let id;
  let isLoading = true;

  let returnable: Components.Schemas.ReturnableDto |undefined = {
    unitPrice: 0,
    name: "",
    vat: 0.0550,
  };
  
  onMount(async () => {
    isLoading = true;
    const result = await getReturnable(id);
    if(!result.success){
      $goto('/returnables/')
      return;      
    }
    
    returnable = result.data;
    isLoading = false;
  });

  const cancelUpdate = () => {
    $goto("/returnables/");
  };

  const updateReturnable = async () => {
    isLoading = true;
    const res = await update(id, returnable);
    if (res.success) {
      $goto(`/returnables/`);
      return;
    }

    isLoading = false;
  };

  $: fullPrice = round(returnable.unitPrice * (1 + returnable.vat / 100));
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
    bind:value="{returnable.code}"
    required="{false}"
    maxLength="{30}"
    placeholder="Le code de votre produit (autogénéré si non renseigné)"
    isLoading="{isLoading}"
  />
  <Text
    label="Nom"
    bind:value="{returnable.name}"
    placeholder="Le nom de votre produit"
    isLoading="{isLoading}"
  />
  <Price
    label="Prix HT"
    bind:value="{returnable.unitPrice}"
    placeholder="Prix HT de votre produit en €"
    isLoading="{isLoading}"
  />
  <Vat label="TVA" bind:value="{returnable.vat}" isLoading="{isLoading}"  rates='{[0, 0.055, 0.10, 0.20]}'/>
  <Price
    label="Prix TTC (calculé)"
    value="{fullPrice}"
    disabled="{true}"
    isLoading="{isLoading}"
    required="{false}"
  />
  <FormFooter
    submit="{updateReturnable}"
    submitText="Mettre à jour"
    cancel="{cancelUpdate}"
    isLoading="{isLoading}"
  />
</Form>
