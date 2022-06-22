<script lang='ts'>
  import { getContext, onMount } from 'svelte'
  import { ListActiveAgreementsQuery } from '$components/Agreements/queries/listActiveAgreements'
  import { mediator } from '$components/mediator'
  import type { Components } from '$types/api'
  import { CreateOrderDraftCommand } from '$components/Orders/commands/createOrderDraft'
  import Select from '$components/Select/Select.svelte'
  import Button from '$components/Button/Button.svelte'
  import { ModalResult } from '$components/Modal/modal'

  export let onClose: Function;
  
  const { close } = getContext("simple-modal");

  let isLoading = true
  let agreements: Components.Schemas.AgreementDto[] = []
  let supplierId: string = null

  const validate = async () => {
    try {
      isLoading = true
      const result = await mediator.send(new CreateOrderDraftCommand(supplierId))
      await onClose(ModalResult.Success(result));
      close();
      isLoading = false
    } catch (exc) {
      isLoading = false
    }
  }
  
  const cancel = async () => {
    await onClose(ModalResult.Failure());
    close();
  }

  onMount(async () => {
    try {
      isLoading = true
      agreements = await mediator.send(new ListActiveAgreementsQuery(1, 100))
      isLoading = false
    } catch (exc) {
      isLoading = false
    }
  })
</script>
<h2 class='mb-4'>Nouvelle commande</h2>
<hr />
<div class='my-6'>
  <Select
    label='Selectionner le fournisseur'
    bind:value={supplierId}
    disabled='{isLoading}'
    options='{agreements.map(a => { return {label: a.targetName, value: a.targetId}})}' />
</div>
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600 mx-2' on:click='{cancel}'>Fermer</Button>
  <Button 
    class='bg-accent-600 mx-2' 
    on:click='{validate}'
    {isLoading}>Créer</Button>
</div>
