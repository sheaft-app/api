<script lang='ts'>
  import { getContext, onMount } from 'svelte'
  import { mediator } from '$components/mediator'
  import Select from '$components/Select/Select.svelte'
  import Button from '$components/Button/Button.svelte'
  import { ModalResult } from '$components/Modal/modal'
  import { GetNextSupplierDeliveryDatesQuery } from '$components/Orders/queries/getNextSupplierDeliveryDates'
  import { format } from 'date-fns'
  import { fr } from 'date-fns/locale'

  export let onClose: Function
  export let supplierId: string

  const { close } = getContext('simple-modal')

  let isLoading = true
  let availableDates: string[] = []
  let deliveryDate: string

  const validate = async () => {
    await onClose(ModalResult.Success(deliveryDate))
    close()
  }

  const cancel = async () => {
    await onClose(ModalResult.Failure())
    close()
  }

  onMount(async () => {
    try {
      isLoading = true
      availableDates = await mediator.send(new GetNextSupplierDeliveryDatesQuery(supplierId))
      isLoading = false
    } catch (exc) {
      isLoading = false
    }
  })
</script>
<h2 class='mb-4'>Dates de livraison possibles</h2>
<hr />
<div class='my-6'>
  <Select
    label='Selectionner une date'
    bind:value={deliveryDate}
    disabled='{isLoading}'
    options='{availableDates.map(a => { return {label: format(new Date(a), "eeee dd MMMM yyyy", { locale: fr }), value: a}})}' />
</div>
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600 mx-2' on:click='{cancel}'>Fermer</Button>
  <Button
    class='bg-accent-600 mx-2'
    on:click='{validate}'
    {isLoading}>Créer
  </Button>
</div>
