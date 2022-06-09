<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import Text from '$components/Inputs/Text.svelte'
  import Price from '$components/Inputs/Price.svelte'
  import Vat from '$components/Inputs/Vat.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import { calculateOnSalePrice } from '$utils/price'
  import { createForm } from 'felte'
  import type { Components } from '$types/api'
  import { mediator } from '$services/mediator'
  import { CreateReturnableRequest } from '$commands/returnables/createReturnable'
  import { getReturnableModule } from '$pages/returnables/module'
  import Button from '$components/Buttons/Button.svelte'

  const module = getReturnableModule($goto)

  const { form, data, isSubmitting, isValid } = createForm<Components.Schemas.CreateReturnableRequest>({
    onSubmit: async (values) => {
      return await mediator.send(new CreateReturnableRequest(values.name, values.unitPrice, values.vat, values.code))
    },
    onSuccess: (id: string) => {
      module.goToDetails(id)
    },
    onError: (result) => {
    }
  })

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat)

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
    id='code'
    label='Code'
    bind:value='{$data.code}'
    required='{false}'
    maxLength='{30}'
    placeholder='Le code de votre consigne (sera autogénéré si non renseigné)'
    isLoading='{$isSubmitting}'
  />
  <Text
    id='name'
    label='Nom'
    bind:value='{$data.name}'
    placeholder='Le nom de votre consigne'
    isLoading='{$isSubmitting}'
  />
  <Price
    id='unitPrice'
    label='Prix HT'
    bind:value='{$data.unitPrice}'
    placeholder='Prix HT de votre consigne en €'
    isLoading='{$isSubmitting}'
  />
  <Vat
    id='vat'
    label='TVA'
    bind:value='{$data.vat}'
    isLoading='{$isSubmitting}'
    rates='{[0, 0.055, 0.10, 0.20]}' />
  <Price
    label='Prix TTC (calculé)'
    value='{onSalePrice}'
    disabled='{true}'
    isLoading='{$isSubmitting}'
    required='{false}'
  />
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
    >Créer
    </Button>
  </FormFooter>
</form>
