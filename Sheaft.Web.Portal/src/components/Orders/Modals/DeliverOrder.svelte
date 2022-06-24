<script lang='ts'>
  import { getContext, onMount } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import { type IModalResult, ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import type { LineQuantity } from '$components/Orders/types'
  import { LineKind } from '$components/Orders/enums'
  import { DeliverOrderCommand } from '$components/Orders/commands/deliverOrder'
  import { ListReturnablesQuery } from '$components/Returnables/queries/listReturnables'

  export let order: Components.Schemas.OrderDetailsDto
  export let onClose: (result: IModalResult<string>) => {}

  const { close } = getContext('simple-modal')

  const lines: LineQuantity[] = order?.lines.filter(l => l.kind == LineKind.Product).map(l => {
    return {
      identifier: l.identifier,
      preparedQuantity: l.quantity,
      quantity: l.quantity,
      name: l.name,
      code: l.code
    }
  })

  let isLoading = true
  let returnedReturnables: LineQuantity[] = []
  let comments: string

  const validate = async () => {
    try {
      isLoading = true
      const result = await mediator.send(new DeliverOrderCommand(order.delivery.id, lines.map(l => {
        return {
          identifier: l.identifier,
          quantity: l.quantity - (<any>l).preparedQuantity
        }
      }), returnedReturnables.map(r => {
        return {
          identifier: r.identifier,
          quantity: -r.quantity
        }
      }), comments))
      close()
      await onClose(ModalResult.Success(result))
    } catch (exc) {
      console.error(exc)
    }
  }

  onMount(async () => {
    try {
      isLoading = true
      const result = await mediator.send(new ListReturnablesQuery(1, 100))
      returnedReturnables = result.map(r => {
        return { identifier: r.id, name: r.name, quantity: 0 }
      })
      isLoading = false
    } catch (exc) {
      onClose(ModalResult.Failure(exc))
    }
  })

</script>

<h2 class='mb-4'>Livrer la commande n°{order.code}</h2>
<hr />
<p class='mt-3'>Cette commande est destinée à {order.customer.name}</p>
<div class='my-6'>
  <table>
    <thead>
    <tr>
      <th>Produit</th>
      <th>Préparé</th>
      <th>Livré</th>
    </tr>
    </thead>
    <tbody>
    {#each lines as line}
      <tr>
        <td>
          <p>{line.name}</p>
          <small>#{line.code}</small>
        </td>
        <td>{line.preparedQuantity}</td>
        <td><input type='number' bind:value={line.quantity} min='0' max='1000' disabled='{isLoading}' /></td>
      </tr>
    {/each}
    </tbody>
  </table>
</div>
<p class='mt-3'>Veuillez indiquer si vous avez récupéré des consignes</p>
<div class='my-6'>
  <table>
    <thead>
    <tr>
      <th>Consigne</th>
      <th>Récupéré</th>
    </tr>
    </thead>
    <tbody>
    {#each returnedReturnables as returnable}
      <tr>
        <td>
          <p>{returnable.name}</p>
        </td>
        <td><input type='number' bind:value={returnable.quantity} min='0' max='1000' disabled='{isLoading}' /></td>
      </tr>
    {/each}
    </tbody>
  </table>
</div>
<div class='my-6'>
  <label for='comments'>Commentaires</label>
  <textarea id='comments' bind:value={comments} disabled='{isLoading}'
            placeholder='Vous pouvez noter les réserves de votre client ici ainsi que le nom de la personne ayant réceptionné votre commande' />
</div>
<hr />
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600' on:click='{close}'>Fermer</Button>
  <Button class='bg-accent-600' on:click='{validate}' disabled='{isLoading}'>Valider</Button>
</div>
<style lang='scss'>
  input {
    max-width: 4em;
    @apply mb-0;
  }

  tbody, tfoot {
    @apply border-b-0;
  }
</style>
