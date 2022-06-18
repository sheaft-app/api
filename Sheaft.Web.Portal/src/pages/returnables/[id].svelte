<script lang='ts'>
  import { page, goto } from '@roxi/routify'
  import { getContext, onMount } from 'svelte'
  import FormFooter from '$components/Form/FormFooter.svelte'
  import { createForm } from 'felte'
  import Button from '$components/Button/Button.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { calculateOnSalePrice } from '$utils/money'
  import { validator } from '@felte/validator-vest'
  import { suite } from '$pages/returnables/validators'
  import reporterDom from '@felte/reporter-dom'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { UpdateReturnableCommand } from '$components/Returnables/commands/updateReturnable'
  import ConfirmRemoveReturnable from '$components/Returnables/Modals/ConfirmRemoveReturnable.svelte'
  import { GetReturnableQuery } from '$components/Returnables/queries/getReturnable'
  import Input from '$components/Input/Input.svelte'
  import Checkbox from '$components/Checkbox/Checkbox.svelte'
  import Vat from '$components/Vat/Vat.svelte'
  import { getReturnableModule } from '$components/Returnables/module'

  export let id:string;
  
  const module = getReturnableModule($goto)
  const { open } = getContext('simple-modal')

  let isLoading = true
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

<PageHeader
  title={$page.title}
  actions={actions}
  previous='{() => module.goToList()}'
/>

<form use:form>
  <Input
    id='name'
    label='Nom'
    bind:value='{$data.name}'
    placeholder='Le nom de votre consigne'
    disabled='{controlsAreDisabled}'
  />
  <Input
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
    <Input
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
      on:click='{module.goToList}'
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
