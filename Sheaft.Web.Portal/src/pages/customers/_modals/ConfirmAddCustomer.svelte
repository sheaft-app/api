<script lang='ts'>
  import { getContext } from 'svelte'
  import Button from '$components/Buttons/Button.svelte'
  import { DayOfWeek } from '$enums/days'
  import Checkbox from '$components/Inputs/Checkbox.svelte'
  import Text from '$components/Inputs/Text.svelte'
  import type { Components } from '$features/api'
  import { mediator } from '$features/mediator'
  import { ProposeAgreementToCustomerCommand } from '$features/agreements/commands/proposeAgreementToCustomer'
  import { address } from '$utils/addresses'

  export let customer: Components.Schemas.AvailableCustomerDto

  const { close } = getContext('simple-modal')

  let deliveryDays = []
  let orderDelayed = false
  let orderDelayInHoursBeforeDeliveryDay: number | null = null

  const validate = async () => {
    try {
      await mediator.send(new ProposeAgreementToCustomerCommand(customer.id, deliveryDays, orderDelayInHoursBeforeDeliveryDay))
    } catch (exc) {
      console.error(exc)
    }
  }
</script>
<div>
  <p>Vous allez envoyer une demande de mise en relation commerciale avec le magasin :</p>
  <div class='m-3'>
    <p><b>{customer.name}</b></p>
    <p>{@html address(customer.deliveryAddress)}</p></div>

  <p>Veuillez selectionner les jours où vous pouvez livrer ce magasin à l'adresse ci-dessus</p>
  <ul class='m-3'>
    <li>
      <label class='form-check-label inline-block text-gray-800 cursor-pointer'>
      <input bind:group={deliveryDays}
             type='checkbox'
             value='{DayOfWeek.Monday}'
             class='w-4 h-4 m-1' />Lundi</label>
    </li>
    <li>
      <label class='form-check-label inline-block text-gray-800 cursor-pointer'>
      <input bind:group={deliveryDays}
             type='checkbox'
             value='{DayOfWeek.Tuesday}'
             class='w-4 h-4 m-1' />Mardi</label>
    </li>
    <li>
      <label class='form-check-label inline-block text-gray-800 cursor-pointer'>
      <input bind:group={deliveryDays}
             type='checkbox'
             value='{DayOfWeek.Wednesday}'
             class='w-4 h-4 m-1' />Mercredi</label>
    </li>
    <li>
      <label class='form-check-label inline-block text-gray-800 cursor-pointer'>
      <input bind:group={deliveryDays}
             type='checkbox'
             value='{DayOfWeek.Thursday}'
             class='w-4 h-4 m-1' />Jeudi</label>
    </li>
    <li>
      <label class='form-check-label inline-block text-gray-800 cursor-pointer'>
      <input bind:group={deliveryDays}
             type='checkbox'
             value='{DayOfWeek.Friday}'
             class='w-4 h-4 m-1' />Vendredi</label>
    </li>
    <li>
      <label class='form-check-label inline-block text-gray-800 cursor-pointer'>
      <input bind:group={deliveryDays}
             type='checkbox'
             value='{DayOfWeek.Saturday}'
             class='w-4 h-4 m-1' />Samedi</label>
    </li>
    <li>
      <label class='form-check-label inline-block text-gray-800 cursor-pointer'>
      <input bind:group={deliveryDays}
             type='checkbox'
             value='{DayOfWeek.Sunday}'
             class='w-4 h-4 m-1' />Dimanche</label>
    </li>
  </ul>
  <div class='mt-5 mb-8'>
    <Checkbox
      id='orderDelay'
      bind:value='{orderDelayed}'
      label='Verrouiller la prise de commandes x heures avant le jour de livraison' />
    {#if orderDelayed}
      <Text type='number' label="Nombre d'heures" bind:value={orderDelayInHoursBeforeDeliveryDay} class='mt-2' />
    {/if}
  </div>
</div>
<div class='flex items-center'>
  <Button class='bg-back-600' on:click={close}>Annuler</Button>
  <Button class='bg-accent-600' on:click={validate}>Ajouter</Button>
</div>
