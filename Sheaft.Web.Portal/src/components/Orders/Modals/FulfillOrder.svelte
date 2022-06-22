<script lang='ts'>
  import { getContext } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import { type IModalResult, ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { FulfillOrderCommand } from '$components/Orders/commands/fulfillOrder'
  import type { DeliveryLine } from '$components/Orders/types'
  import { OrderLineKind } from '$components/Orders/enums'

  export let order: Components.Schemas.OrderDetailsDto
  export let onClose: (result: IModalResult<string>) => {}

  const { close } = getContext('simple-modal')

  const lines: DeliveryLine[] = order?.lines.filter(l => l.kind == OrderLineKind.Product).map(l => {
    return {
      productIdentifier: l.identifier,
      orderedQuantity: l.orderedQuantity,
      quantity: l.orderedQuantity,
      name: l.name,
      code: l.code
    }
  })

  const validate = async () => {
    try {
      const result = await mediator.send(new FulfillOrderCommand(order.id, lines))
      close()
      await onClose(ModalResult.Success(result))
    } catch (exc) {
      console.error(exc)
    }
  }

</script>

<h2 class='mb-4'>Préparer la commande n°{order.code}</h2>
<hr />
<div class='my-6'>
  <table>
    <thead>
    <tr>
      <th>Nom</th>
      <th>Commandé</th>
      <th>Préparé</th>
    </tr>
    </thead>
    <tbody>
    {#each lines as line}
      <tr>
        <td>
          <p>{line.name}</p>
          <small>#{line.code}</small>
        </td>
        <td>{line.orderedQuantity}</td>
        <td><input type='number' bind:value={line.quantity} min='0' max='1000'/></td>
      </tr>
    {/each}
    </tbody>
  </table>
</div>
<hr />
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600' on:click='{close}'>Fermer</Button>
  <Button class='bg-accent-600' on:click='{validate}'>Valider</Button>
</div>
<style lang='scss'>
  input{
    max-width: 4em;
    @apply mb-0;
  }
  
  tbody, tfoot{
    @apply border-b-0;
  }
</style>
