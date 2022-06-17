<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import Text from '$components/Inputs/Text.svelte'
  import TextArea from '$components/Inputs/TextArea.svelte'
  import Vat from '$components/Inputs/Vat.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import Select from '$components/Inputs/Select.svelte'
  import Checkbox from '$components/Inputs/Checkbox.svelte'
  import { getContext, onMount } from 'svelte'
  import Button from '$components/Buttons/Button.svelte'
  import { createForm } from 'felte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { getProductModule } from '$features/products/module'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { UpdateProductCommand } from '$features/products/commands/updateProduct'
  import { calculateOnSalePrice } from '$utils/money'
  import { GetProductQuery } from '$features/products/queries/getProduct'
  import { ListReturnablesOptionsQuery } from '$features/products/queries/listReturnablesOptions'
  import ConfirmRemoveProduct from '$pages/products/_ConfirmRemoveProduct.svelte'
  import { validator } from '@felte/validator-vest'
  import { suite } from '$pages/products/validators'
  import reporterDom from '@felte/reporter-dom'

  export let id = ''
  const module = getProductModule($goto)
  const { open } = getContext('simple-modal')

  let isLoading = false
  let isReturnable = false
  let returnablesOptions: { label: string; value: string }[] = []

  const { form, data, isSubmitting, isValid, setData } =
    createForm<Components.Schemas.UpdateProductRequest>({
      onSubmit: async (values) => {
        return await mediator.send(
          new UpdateProductCommand(
            id,
            values.name,
            values.unitPrice,
            values.vat,
            values.code,
            values.description,
            values.returnableId
          )
        )
      },
      onSuccess: () => {
        module.goToProductList()
      },
      extend: [
        <any>validator({ suite }),
        reporterDom({ single: true })
      ]
    })

  const onClose = () => {
    module.goToProductList()
  }

  const confirmModal = () => {
    open(ConfirmRemoveProduct, {
        productId: id,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: true
      })
  }

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat)
  $: controlsAreDisabled = isLoading || $isSubmitting;
  
  onMount(async () => {
    try {
      isLoading = true

      const product = await mediator.send(new GetProductQuery(id))
      isReturnable = !!product.returnableId
      setData(product)

      returnablesOptions = await mediator.send(new ListReturnablesOptionsQuery())

      isLoading = false
    } catch (exc) {
      module.goToProductList()
    }
  })

  $: actions = [
    {
      name: 'Supprimer',
      disabled: controlsAreDisabled,
      color: 'danger',
      action: () => confirmModal()
    }
  ]
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details du produit" -->
<!-- routify:options roles=[] -->

<PageHeader
  title={$page.title}
  actions={actions}
  previous='{() => module.goToProductList()}'
/>

<form use:form>
  <Text
    id='code'
    label='Code'
    bind:value='{$data.code}'
    required='{false}'
    maxLength='{30}'
    placeholder='Le code de votre produit (sera autogénéré si non renseigné)'
    disabled='{controlsAreDisabled}'
  />
  <Text
    id='name'
    label='Nom'
    bind:value='{$data.name}'
    placeholder='Le nom de votre produit'
    disabled='{controlsAreDisabled}'
  />
  <Text
    id='unitPrice'
    label='Prix HT'
    bind:value='{$data.unitPrice}'
    placeholder='Prix HT de votre produit en €'
    disabled='{controlsAreDisabled}'
  />
  <Vat id='vat' label='TVA' bind:value='{$data.vat}' disabled='{controlsAreDisabled}' />
  <Text
    type='number'
    label='Prix TTC (calculé)'
    value='{onSalePrice}'
    disabled='{true}'
    required='{false}'
  />
  <TextArea
    id='description'
    label='Description'
    bind:value='{$data.description}'
    placeholder='Les ingrédients, la méthode de préparation, tout ce que vous pouvez juger utile de préciser'
    disabled='{controlsAreDisabled}'
  />
  <Checkbox
    id='hasReturnable'
    label='Ce produit est consigné'
    disabled='{controlsAreDisabled}'
    bind:value='{isReturnable}'
    class='mt-3 mb-6'
  />
  {#if isReturnable}
    <Select
      id='returnableId'
      label='Consigne'
      options='{returnablesOptions}'
      disabled='{controlsAreDisabled}'
      bind:value='{$data.returnableId}'
    />
  {/if}
  <FormFooter>
    <Button
      type='button'
      disabled='{controlsAreDisabled}'
      class='back w-full mx-8'
      on:click='{module.goToProductList}'
    >Revenir à la liste
    </Button>
    <Button
      type='submit'
      disabled='{controlsAreDisabled}'
      isLoading='{$isSubmitting}'
      class='accent w-full mx-8'
    >Sauvegarder
    </Button>
  </FormFooter>
</form>
