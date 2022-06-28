<script lang='ts'>
  import { getContext, onMount } from 'svelte'
  import Button from '$components/Button/Button.svelte'
  import { type IModalResult, ModalResult } from '$components/Modal/modal'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { FulfillOrderCommand } from '$components/Orders/commands/fulfillOrder'
  import type { BatchSelection, DeliveryLine, DeliveryLineCompletion } from '$components/Orders/types'
  import { LineKind } from '$components/Orders/enums'
  import Select from 'svelte-select'
  import { ListBatchesQuery } from '$components/Batches/queries/listBatches'
  import { dateStr } from '$utils/dates'

  export let order: Components.Schemas.OrderDetailsDto
  export let onClose: (result: IModalResult<string>) => {}

  const { close } = getContext('simple-modal')

  const lines: DeliveryLineCompletion[] = order?.lines.filter(l => l.kind == LineKind.Product).map(l => {
    return {
      productIdentifier: l.identifier,
      orderedQuantity: l.quantity,
      quantity: l.quantity,
      name: l.name,
      code: l.code,
      batchIdentifiers: []
    }
  })

  let isLoading = true
  let batches: BatchSelection[] = []
  let batchesToApply: BatchSelection[] = []

  const getProductBatchIdentifiers = (productBatchSelections: BatchSelection[]) => {
    if (!productBatchSelections)
      return batchIdentifiers

    const productBatchIdentifiers = productBatchSelections?.map((bi: any) => {
      return bi.value
    })

    console.log(productBatchIdentifiers, batchIdentifiers);
    let res = [...productBatchIdentifiers, ...batchIdentifiers];
    console.log(res)
    return res;
  }

  const validate = async () => {
    try {
      isLoading = true
      const result = await mediator.send(new FulfillOrderCommand(order.id, lines.map(l => {
        return {
          ...l, batchIdentifiers: getProductBatchIdentifiers(l.batchIdentifiers)
        }
      })))
      await onClose(ModalResult.Success(result))
      close()
    } catch (exc) {
      isLoading = false
      console.error(exc)
    }
  }

  onMount(async () => {
    try {
      isLoading = true
      const result = await mediator.send(new ListBatchesQuery(1, 100))
      batches = result.map(r => {
        return { label: `${r.number} - ${dateStr(r.expirationDate)}`, value: r.id }
      })
      isLoading = false
    } catch (exc) {
      console.error(exc)
    }
  })

  $: batchIdentifiers = batchesToApply.map(b => {
    return b.value
  })

</script>

<h2 class='mb-4'>Préparer la commande n°{order.code}</h2>
<hr />
<p class='mt-3'>Cette commande est destinée à {order.customer.name}</p>
<div class='my-6'>
  <table>
    <thead>
    <tr>
      <th>Produit</th>
      <th>Commandé</th>
      <th>Préparé</th>
      <th class='w-80'>Lots</th>
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
        <td><input type='number' bind:value={line.quantity} min='0' max='1000' disabled='{isLoading}' /></td>
        <td><Select items='{batches}' bind:value='{line.batchIdentifiers}' isMulti='{true}'
                    noOptionsMessage='Aucun lot disponible' placeholder='Assigner un/des lot(s) spécifique(s)'></Select>
        </td>
      </tr>
    {/each}
    </tbody>
    <tfoot>
    <tr>
      <td colspan='4'>
        <label class='pb-1 block'>Ces lots seront appliqués à tous les produits de la commande</label>
        <Select id='batchIdentifiers' items='{batches}' bind:value='{batchesToApply}' isMulti='{true}'
                noOptionsMessage='Aucun lot disponible'
                placeholder='Assigner un ou plusieurs lot(s) à tous les produits'></Select>
      </td>
    </tr>
    </tfoot>
  </table>
</div>
<hr />
<div class='flex items-center justify-evenly pt-4'>
  <Button class='bg-default-600' on:click='{close}' disabled='{isLoading}'>Fermer</Button>
  <Button class='bg-accent-600' on:click='{validate}' {isLoading}>Valider</Button>
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
