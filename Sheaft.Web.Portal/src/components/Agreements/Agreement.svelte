<script lang='ts'>
  import { getContext, onMount } from 'svelte'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { GetAgreementQuery } from '$components/Agreements/queries/getAgreement'
  import { AgreementOwner, AgreementStatus } from '$components/Agreements/enums'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import RefuseAgreement from '$components/Agreements/Modals/RefuseAgreement.svelte'
  import AcceptSupplierAgreement from '$components/Agreements/Modals/AcceptSupplierAgreement.svelte'
  import AcceptCustomerAgreement from '$components/Agreements/Modals/AcceptCustomerAgreement.svelte'
  import CancelAgreement from '$components/Agreements/Modals/CancelAgreement.svelte'
  import RevokeAgreement from '$components/Agreements/Modals/RevokeAgreement.svelte'
  import Input from '$components/Input/Input.svelte'
  import Address from '$components/Address/Address.svelte'
  import { authStore } from '$components/Account/store'
  import { ProfileKind } from '$components/Account/enums'
  import DeliveryDays from '$components/DeliveryDays/DeliveryDays.svelte'
  import DeliveryOffsetHour from '$components/DeliveryOffsetHours/DeliveryOffsetHour.svelte'
  import Button from '$components/Button/Button.svelte'
  import { UpdateAgreementDeliveryCommand } from '$components/Agreements/commands/updateAgreement'
  import { createForm } from 'felte'
  import { getFormValidators } from '$components/validate'
  import { suite } from '$components/Agreements/validators'
  import type { AgreementDeliveryForm } from '$components/Agreements/types'
  import type { IAgreementModule } from '$components/Agreements/module'
  import { CreateOrderDraftCommand } from '$components/Orders/commands/createOrderDraft'
  import { getOrderModule } from '$components/Orders/module'
  import { goto } from '@roxi/routify'

  export let id: string
  export let title: string = "Détails de l'accord commercial"  
  export let module: IAgreementModule; 
  
  const orderModule = getOrderModule($goto);
  const { open } = getContext('simple-modal')

  let agreement: Components.Schemas.AgreementDetailsDto = null

  const createOrderDraft = async () =>{
    try{
      const result = await mediator.send(new CreateOrderDraftCommand(agreement?.target.id));
      orderModule.goToDraft(result);
    }
    catch(exc){
      console.error(exc);
    }
  }
  
  const openAcceptModal = () => {
    openModal(
      agreement.owner == AgreementOwner.Supplier
        ? AcceptSupplierAgreement
        : AcceptCustomerAgreement
    )
  }

  const openRefuseModal = () => {
    openModal(RefuseAgreement)
  }

  const openCancelModal = () => {
    openModal(CancelAgreement)
  }

  const openRevokeModal = () => {
    openModal(RevokeAgreement)
  }

  const onClose = () => {
    module.goToList()
  }

  const openModal = Modal => {
    open(
      Modal,
      {
        agreement,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: false
      }
    )
  }

  const onSubmit = async (values) => {
    try {
      const result = await mediator.send(
        new UpdateAgreementDeliveryCommand(
          agreement.id,
          values.deliveryDays,
          values.limitOrderHourOffset
        )
      )
    } catch (exc) {
      console.error(exc)
    }
  }
  
  const onSuccess = () => {
    module.goToList()
  }
  
  const loadAgreement = async () => {
    try {
      isLoading = true
      agreement = await mediator.send(new GetAgreementQuery(id))

      const agreementForm = <AgreementDeliveryForm>agreement
      agreementForm.limitEnabled = agreementForm.limitOrderHourOffset > 0
      setInitialValues(agreementForm)
      setData(agreementForm)
      title = agreement.target.name
      isLoading = false
    } catch (exc) {
      module.goToList()
    }
  }

  const { form, data, isSubmitting,  setData, setInitialValues } = createForm<AgreementDeliveryForm>({
    onSubmit,
    onSuccess,
    extend: getFormValidators(suite)
  })

  onMount(async () => {
    await loadAgreement();
  })

  let isLoading = false

  $: actions = [
    {
      name: 'Commander',
      disabled: isLoading,
      visible: agreement?.canBeRevoked && $authStore.account?.profile?.kind == ProfileKind.Customer,
      color: 'accent',
      action: () => createOrderDraft()
    },
    {
      name: 'Accepter',
      disabled: isLoading,
      visible: agreement?.canBeAcceptedOrRefused,
      color: 'success',
      action: () => openAcceptModal()
    },
    {
      name: 'Refuser',
      disabled: isLoading,
      visible: agreement?.canBeAcceptedOrRefused,
      color: 'danger',
      action: () => openRefuseModal()
    },
    {
      name: 'Annuler la demande',
      disabled: isLoading,
      visible: agreement?.canBeCancelled,
      color: 'warning',
      action: () => openCancelModal()
    },
    {
      name: 'Revoquer l\'accord',
      disabled: isLoading,
      visible: agreement?.canBeRevoked,
      color: 'warning',
      action: () => openRevokeModal()
    }
  ]
</script>

<PageHeader
  title='{title}'
  actions='{actions}'
  previous='{() => module.goToList()}'
  class='max-w-4xl' />

<div class='max-w-xl'>
  <h2 class='my-6'>Information</h2>
  <Input label='Nom' value='{agreement?.target.name}' disabled='{true}' />
  <Input type='email' label='Adresse mail' value='{agreement?.target.email}' disabled='{true}' />
  <Input type='tel' label='Numéro de téléphone' value='{agreement?.target.phone}' disabled='{true}' />
  <h2 class='my-6'>Livraison</h2>
  <Address
    label="{$authStore.account?.profile?.kind === ProfileKind.Customer ? 'Vous livrera à cette adresse' : 'Adresse de livraison'}"
    disabled='{true}' value='{agreement?.deliveryAddress}' />
  {#if $authStore.account?.profile?.kind == ProfileKind.Supplier}
    <form use:form>
      <DeliveryDays
        id='deliveryDays'
        bind:days={$data.deliveryDays} />
      <DeliveryOffsetHour id='limitOrderHourOffset'
                          toggleId='limitEnabled'
                          label='La commande doit être passée au minimum {$data.limitOrderHourOffset > 0 ? $data.limitOrderHourOffset : "x"} heures avant le jour de livraison'
                          bind:offset={$data.limitOrderHourOffset}
                          bind:offsetEnabled={$data.limitEnabled} />

      <div class='flex items-center justify-evenly pt-4'>
        <Button class='bg-default-600' type='reset' disabled='{$isSubmitting || isLoading}'>Annuler</Button>
        <Button class='bg-accent-600' type='submit' isLoading='{$isSubmitting}' disabled='{$isSubmitting || isLoading}'>Sauvegarder</Button>
      </div>
    </form>
  {:else if agreement?.status == AgreementStatus.Active}
    <DeliveryDays
      days={agreement?.deliveryDays}
      disabled='{true}'
      message='Jours de livraison' />
    <label>Vous devez passer commande {agreement?.limitOrderHourOffset} heures avant le jour de livraison</label>
  {/if}
</div>
