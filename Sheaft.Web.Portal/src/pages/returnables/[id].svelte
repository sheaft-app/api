<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { onMount } from 'svelte'
  import Text from '$components/Inputs/Text.svelte'
  import Price from '$components/Inputs/Price.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import { createForm } from 'felte'
  import { calculateOnSalePrice } from '$utils/price'
  import Vat from '$components/Inputs/Vat.svelte'
  import { mediator } from '$services/mediator'
  import { UpdateReturnableRequest } from '$commands/returnables/updateReturnable'
  import { getReturnableModule } from '$pages/returnables/module'
  import { GetReturnableQuery } from '$queries/returnables/getReturnable'
  import type { Components } from '$types/api'
  import Button from '$components/Buttons/Button.svelte'

  export let id
  let initializing = true
  const module = getReturnableModule($goto)

  const { form, data, isSubmitting, isValid, setData } = createForm<Components.Schemas.UpdateReturnableRequest>({
    onSubmit: async (values) => {
      await mediator.send(new UpdateReturnableRequest(id, values.name, values.unitPrice, values.vat, values.code))
    },
    onSuccess: () => {
      module.goToList()
    },
    onError: (result) => {

    }
  })

  onMount(async () => {
    initializing = true
    try {
      const result = await mediator.send(new GetReturnableQuery(id))
      setData(result)
      initializing = false
    }
    catch(exc){ 
      console.error(exc);
      module.goToList();
    }
  })

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat)
  $: isLoading = (isSubmitting || initializing)
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
    label='Code'
    bind:value='{$data.code}'
    required='{false}'
    maxLength='{30}'
    placeholder='Le code de votre produit (autogénéré si non renseigné)'
    isLoading='{$isLoading}'
  />
  <Text
    id='name'
    label='Nom'
    bind:value='{$data.name}'
    placeholder='Le nom de votre produit'
    isLoading='{$isLoading}'
  />
  <Price
    id='unitPrice'
    label='Prix HT'
    bind:value='{$data.unitPrice}'
    placeholder='Prix HT de votre produit en €'
    isLoading='{$isLoading}'
  />
  <Vat
    id='vat'
    label='TVA'
    bind:value='{$data.vat}'
    isLoading='{$isLoading}'
    rates='{[0, 0.055, 0.10, 0.20]}' />
  <Price
    id='onSalePrice'
    label='Prix TTC (calculé)'
    value='{onSalePrice}'
    disabled='{true}'
    isLoading='{$isLoading}'
    required='{false}'
  />
  <FormFooter>
    <Button
      type='button'
      disabled='{$isLoading}'
      class='back w-full mx-8'
      on:click='{module.goToList}'
    >Annuler
    </Button>
    <Button
      type='submit'
      disabled='{$isLoading}'
      isLoading='{$isLoading}'
      class='accent w-full mx-8'
    >Sauvegarder
    </Button>
  </FormFooter>
</form>
