<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { getContext, onMount } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import { createForm } from 'felte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { calculateOnSalePrice } from '$utils/money'
  import { validator } from '@felte/validator-vest'
  import { suite } from '$pages/products/validators'
  import reporterDom from '@felte/reporter-dom'
  import { getProductModule } from '$components/Products/module'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { UpdateProductCommand } from '$components/Products/commands/updateProduct'
  import ConfirmRemoveProduct from '$components/Products/Modals/ConfirmRemoveProduct.svelte'
  import { GetProductQuery } from '$components/Products/queries/getProduct'
  import { ListReturnablesOptionsQuery } from '$components/Products/queries/listReturnablesOptions'
  import Input from '$components/Input/Input.svelte'
  import TextArea from '$components/TextArea/TextArea.svelte'
  import Vat from '$components/Vat/Vat.svelte'
  import Checkbox from '$components/Checkbox/Checkbox.svelte'
  import Select from '$components/Select/Select.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'

  export let id:string;
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
        module.goToList()
      },
      extend: [
        <any>validator({ suite }),
        reporterDom({ single: true })
      ]
    })

  const onClose = () => {
    module.goToList()
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
      module.goToList()
    }
  })

  $: actions = [
    {
      name: 'Supprimer',
      disabled: controlsAreDisabled,
      visible: true,
      color: 'danger',
      action: () => confirmModal()
    }
  ]
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details du produit" -->

<PageHeader
  title={$page.title}
  actions={actions}
  previous='{() => module.goToList()}'
/>

<form use:form>
  <Input
    id='code'
    label='Code'
    bind:value='{$data.code}'
    required='{false}'
    maxLength='{30}'
    placeholder='Le code de votre produit (sera autogénéré si non renseigné)'
    disabled='{controlsAreDisabled}'
  />
  <Input
    id='name'
    label='Nom'
    bind:value='{$data.name}'
    placeholder='Le nom de votre produit'
    disabled='{controlsAreDisabled}'
  />
  <Input
    id='unitPrice'
    label='Prix HT'
    bind:value='{$data.unitPrice}'
    placeholder='Prix HT de votre produit en €'
    disabled='{controlsAreDisabled}'
  />
  <Vat id='vat' label='TVA' bind:value='{$data.vat}' disabled='{controlsAreDisabled}' />
  <Input
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
      on:click='{module.goToList}'
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
