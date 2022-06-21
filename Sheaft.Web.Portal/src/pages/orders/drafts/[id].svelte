<script lang='ts'>
  import { goto, page } from '@roxi/routify'
  import { onMount } from 'svelte'
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
  import { dateStr } from '$utils/dates'
  import { calculateOnSalePrice, calculateVatPrice, calculateWholeSalePrice, currency } from '$utils/money'
  import {formatInnerHtml} from "$actions/format"
  import { percent } from '$utils/percent'
  import { UpdateOrderDraftProductsCommand } from '$components/Orders/commands/updateOrderDraftProducts'

  export let id: string
  const module = getOrderModule($goto)

  let title = $page.title
  let order: Components.Schemas.OrderDraftDto = null
  let products: Components.Schemas.OrderableProductDto[] = []
  let lines: DraftLine[] = []
  let deliveryDate: Date

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
  
  const publishOrderDraft = async () => {
    try {
      isLoading = true
      await mediator.send(new PublishOrderDraftCommand(id, dateStr(deliveryDate, 'yyyy-MM-dd'), lines.filter(l => l.quantity > 0).map(l => {
        return { productIdentifier: l.id, quantity: l.quantity }
      })))
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
      module.goToList()
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
      action: () => publishOrderDraft()
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
      <tr>
        <td><p class='font-medium'>{line.name}</p>
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
      <tr>
        <td class='font-medium'>{returnable.name}</td>
        <td use:formatInnerHtml={currency}>{returnable.unitPrice}</td>
        <td use:formatInnerHtml={percent}>{returnable.vat}</td>
        <td>{returnable.quantity}</td>
        <td>{currency(calculateOnSalePrice(returnable.unitPrice, returnable.vat, returnable.quantity))}</td>
      </tr>
    {/each}
    </tbody>
    <tfoot>
      <tr class='hover:bg-white cursor-default'>
        <td colspan='4' class='text-right font-medium'>Total HT</td>
        <td class='font-medium'>{currency(totalWholeSalePrice)}</td>
      </tr>
      <tr class='hover:bg-white cursor-default'>
        <td colspan='4' class='text-right font-medium'>Total TVA</td>
        <td class='font-medium'>{currency(totalVatPrice)}</td>
      </tr>
      <tr class='hover:bg-white cursor-default'>
        <td colspan='4' class='text-right font-medium'>Total TTC</td>
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
