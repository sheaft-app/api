<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { getContext, onMount } from 'svelte'
  import Text from '$components/Inputs/Text.svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import { createForm } from 'felte'
  import Vat from '$components/Inputs/Vat.svelte'
  import Button from '$components/Buttons/Button.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { getProductModule } from '$features/products/module'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { UpdateReturnableCommand } from '$features/products/commands/updateReturnable'
  import { GetReturnableQuery } from '$features/products/queries/getReturnable'
  import { calculateOnSalePrice } from '$utils/money'
  import ConfirmRemoveReturnable from './_ConfirmRemoveReturnable.svelte'
  import { validator } from '@felte/validator-vest'
  import { suite } from '$pages/returnables/validators'
  import reporterDom from '@felte/reporter-dom'
  import Checkbox from '$components/Inputs/Checkbox.svelte'

  export let id = ''
  let isLoading = true
  const module = getProductModule($goto)
  const { open } = getContext('simple-modal')

  let hasVat = false

  const { form, data, isSubmitting, isValid, setData } =
    createForm<Components.Schemas.UpdateReturnableRequest>({
      onSubmit: async values => {
        await mediator.send(
          new UpdateReturnableCommand(
            id,
            values.name,
            values.unitPrice,
            values.vat
          )
        )
      },
      onSuccess: () => {
        module.goToReturnableList()
      },
      extend: [
        <any>validator({ suite }),
        reporterDom({ single: true })
      ]
    })

  const onClose = () => {
    module.goToReturnableList()
  }

  const confirmModal = () => {
    open(ConfirmRemoveReturnable, {
        returnableId: id,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: true
      })
  }

  onMount(async () => {
    try {
      isLoading = true
      const returnable = await mediator.send(new GetReturnableQuery(id))
      setData(returnable)
      hasVat = returnable.vat > 0
      isLoading = false
    } catch (exc) {
      console.error(exc)
      module.goToReturnableList()
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

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat)
  $: controlsAreDisabled = isLoading || $isSubmitting

  $: if (hasVat && (!$data.vat || $data.vat == 0)){
    $data.vat = 5.5
  } else if (!hasVat && $data.vat && $data.vat > 0) {
    $data.vat = 0
  }
</script>

<!-- routify:options index=true -->
<!-- routify:options title="Details de la consigne" -->
<!-- routify:options roles=[] -->

<PageHeader
  title={$page.title}
  actions={actions}
  previous='{() => module.goToReturnableList()}'
/>

<form use:form>
  <Text
    id='name'
    label='Nom'
    bind:value='{$data.name}'
    placeholder='Le nom de votre consigne'
    disabled='{controlsAreDisabled}'
  />
  <Text
    id='unitPrice'
    label='Prix HT'
    bind:value='{$data.unitPrice}'
    placeholder='Prix HT de votre consigne en €'
    disabled='{controlsAreDisabled}'
  />
  <Checkbox
    id='hasVat'
    label='Je facture la TVA pour cette consigne'
    disabled='{controlsAreDisabled}'
    bind:value='{hasVat}'
    class='my-3'
  />
  {#if hasVat}
    <Vat
      id='vat'
      label='TVA'
      bind:value='{$data.vat}'
      disabled='{controlsAreDisabled}'
    />
    <Text
      id='onSalePrice'
      type='number'
      label='Prix TTC (calculé)'
      value='{onSalePrice}'
      disabled='{true}'
      required='{false}'
    />
  {/if}
  <FormFooter>
    <Button
      type='button'
      disabled='{controlsAreDisabled}'
      class='back w-full mx-8'
      on:click='{module.goToReturnableList}'
    >Annuler
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
