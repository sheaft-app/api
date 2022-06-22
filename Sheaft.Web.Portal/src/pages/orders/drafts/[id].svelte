<script lang='ts'>
  import { goto, page } from '@roxi/routify'
  import { getContext, onMount } from 'svelte'
  import { getOrderModule } from '$components/Orders/module'
  import { GetOrderDraftQuery } from '$components/Orders/queries/getOrderDraft'
  import { mediator } from '$components/mediator'
  import type { Components } from '$types/api'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import { ListSupplierOrderableProductsQuery } from '$components/Orders/queries/listOrderableProducts'
  import type { DraftLine } from '$components/Orders/types'
  import type { PageAction } from '$components/Page/types'
  import { PublishOrderDraftCommand } from '$components/Orders/commands/publishOrderDraft'
  import { calculateOnSalePrice, calculateVatPrice, calculateWholeSalePrice, currency } from '$utils/money'
  import {formatInnerHtml} from "$actions/format"
  import { percent } from '$utils/percent'
  import { UpdateOrderDraftProductsCommand } from '$components/Orders/commands/updateOrderDraftProducts'
  import SelectDeliveryDate from '$components/Orders/Modals/SelectDeliveryDate.svelte'
  import type { IModalResult } from '$components/Modal/modal'
  import { OrderTab } from '$components/Orders/enums'

  export let id: string
  const module = getOrderModule($goto)
  const { open } = getContext('simple-modal')

  let title = $page.title
  let order: Components.Schemas.OrderDraftDto = null
  let products: Components.Schemas.OrderableProductDto[] = []
  let lines: DraftLine[] = []

  const updateOrderDraftProducts = async () => {
    try {
      isLoading = true
      await mediator.send(new UpdateOrderDraftProductsCommand(id, lines.filter(l => l.quantity > 0).map(l => {
        return { productIdentifier: l.id, quantity: l.quantity }
      })))
      isLoading = false
    } catch (exc) {
      console.error(exc)
    }
  }
  
  const openSelectDeliveryDate = () => {
    open(
      SelectDeliveryDate,
      {
        supplierId: order.supplier.id,
        onClose:publishOrderDraft
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: true
      }
    )
  }
  
  const publishOrderDraft = async (result:IModalResult<string>) => {    
    if(!result.isSuccess)
      return;
    
    try {
      isLoading = true
      await mediator.send(new PublishOrderDraftCommand(id, result.value, lines.filter(l => l.quantity > 0).map(l => {
        return { productIdentifier: l.id, quantity: l.quantity }
      })))
      
      module.goToList({tab:OrderTab.Pending});
      isLoading = false
    } catch (exc) {
      console.error(exc)
    }
  }

  onMount(async () => {
    try {
      isLoading = true
      order = await mediator.send(new GetOrderDraftQuery(id))
      title = `Commande pour ${order.supplier.name}`
      products = await mediator.send(new ListSupplierOrderableProductsQuery(order.supplier.id))
      lines = products.map(p => {
        const orderLine = order.lines.filter(l => l.identifier == p.id)[0]
        return {
          id: p.id,
          name: p.name,
          code: p.code,
          quantity: orderLine?.quantity ?? 0,
          vat: p.vat,
          unitPrice: p.unitPrice,
          returnable: p.returnable
        }
      })
      isLoading = false
    } catch (exc) {
      module.goToList({tab:OrderTab.Draft})
    }
  })

  const actions: PageAction[] = [
    {
      name: 'Enregistrer',
      disabled: isLoading,
      visible: true,
      color: 'accent',
      action: () => updateOrderDraftProducts()
    },
    {
      name: 'Envoyer',
      disabled: isLoading,
      visible: true,
      color: 'primary',
      action: () => openSelectDeliveryDate()
    }
  ]

  $: isLoading = true
  
  $:returnables = lines.filter(l => l.returnable).map(l => {
    return { ...l.returnable, quantity: l.quantity }
  })
  
  $: totalWholeSalePrice = lines.reduce((acc:number, line:DraftLine) => 
    acc + calculateWholeSalePrice(line.unitPrice, line.quantity) + calculateWholeSalePrice(line.returnable?.unitPrice, line.quantity), 0);
  
  $: totalVatPrice = lines.reduce((acc:number, line:DraftLine) =>
    acc + calculateVatPrice(line.unitPrice, line.vat, line.quantity) + calculateVatPrice(line.returnable?.unitPrice, line.returnable?.vat, line.quantity), 0);
  
  $: totalOnSalePrice = lines.reduce((acc:number, line:DraftLine) =>
    acc + calculateOnSalePrice(line.unitPrice, line.vat, line.quantity) + calculateOnSalePrice(line.returnable?.unitPrice, line.returnable?.vat, line.quantity), 0);
</script>

<!-- routify:options title="Brouillon de commande" -->
<!-- routify:options index=true -->

<PageHeader {title} {actions} />

<PageContent>
  <table>
    <thead>
    <tr>
      <th>Nom</th>
      <th>Prix unitaire (HT)</th>
      <th>TVA</th>
      <th>Quantité</th>
      <th>Prix total (TTC)</th>
    </tr>
    </thead>
    <tbody>
    <tr class='bg-gray-50 hover:bg-gray-50 cursor-default'>
      <th colspan='5' class='font-bold text-gray-600'>Produits</th>
    </tr>
    {#each lines ?? [] as line}
      <tr class='cursor-default'>
        <td><p class='font-semibold'>{line.name}</p>
          <small>#{line.code}</small></td>
        <td use:formatInnerHtml={currency}>{line.unitPrice}</td>
        <td use:formatInnerHtml={percent}>{line.vat}</td>
        <td><input class='m-0' type='number' bind:value={line.quantity} min='0'/></td>
        <td>{currency(calculateOnSalePrice(line.unitPrice, line.vat, line.quantity))}</td>
      </tr>
    {/each}
    <tr class='bg-gray-50 hover:bg-gray-50 cursor-default'>
      <th colspan='5' class='font-bold text-gray-600'>Consignes</th>
    </tr>
    {#each returnables ?? [] as returnable}
      <tr class='cursor-default'>
        <td class='font-semibold'>{returnable.name}</td>
        <td use:formatInnerHtml={currency}>{returnable.unitPrice}</td>
        <td use:formatInnerHtml={percent}>{returnable.vat}</td>
        <td>{returnable.quantity}</td>
        <td>{currency(calculateOnSalePrice(returnable.unitPrice, returnable.vat, returnable.quantity))}</td>
      </tr>
    {/each}
    </tbody>
    <tfoot>
      <tr class='hover:bg-white cursor-default'>
        <td colspan='4' class='text-right font-semibold'>Total HT</td>
        <td class='font-medium'>{currency(totalWholeSalePrice)}</td>
      </tr>
      <tr class='hover:bg-white cursor-default'>
        <td colspan='4' class='text-right font-semibold'>Total TVA</td>
        <td class='font-medium'>{currency(totalVatPrice)}</td>
      </tr>
      <tr class='hover:bg-white cursor-default'>
        <td colspan='4' class='text-right font-semibold'>Total TTC</td>
        <td class='font-medium'>{currency(totalOnSalePrice)}</td>
      </tr>
    </tfoot>
  </table>
</PageContent>

<style>
  input {
    @apply m-0;
    max-width: 4em;
  }
</style>
