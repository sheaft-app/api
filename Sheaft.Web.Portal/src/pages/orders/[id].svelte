<script lang='ts'>
  import { goto, page } from '@roxi/routify'
  import { getContext, onMount } from 'svelte'
  import { getOrderModule } from '$components/Orders/module'
  import { mediator } from '$components/mediator'
  import type { Components } from '$types/api'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import PageContent from '$components/Page/PageContent.svelte'
  import { currency } from '$utils/money'
  import { formatInnerHtml } from '$actions/format'
  import { percent } from '$utils/percent'
  import { DeliveryStatus, LineKind, OrderStatus } from '$components/Orders/enums'
  import { GetOrderQuery } from '$components/Orders/queries/getOrder'
  import type { IModalResult } from '$components/Modal/modal'
  import AcceptOrder from '$components/Orders/Modals/AcceptOrder.svelte'
  import RefuseOrder from '$components/Orders/Modals/RefuseOrder.svelte'
  import CancelOrder from '$components/Orders/Modals/CancelOrder.svelte'
  import FulfillOrder from '$components/Orders/Modals/FulfillOrder.svelte'
  import { dateStr } from '$utils/dates'
  import { orderStatus } from '$components/Orders/utils'
  import { ProfileKind } from '$components/Account/enums'
  import { authStore } from '$components/Account/store'
  import DeliverOrder from '$components/Orders/Modals/DeliverOrder.svelte'
  import InvoiceDelivery from '$components/Orders/Modals/InvoiceDelivery.svelte'

  export let id: string
  const module = getOrderModule($goto)
  const { open } = getContext('simple-modal')

  let title = $page.title
  let order: Components.Schemas.OrderDetailsDto = null

  let products = []
  let returnables = []
  let returnedReturnables = []
  let totalWholeSalePrice = 0
  let totalVatPrice = 0
  let totalOnSalePrice = 0

  const acceptOrder = () => {
    openModal(AcceptOrder)
  }
  const refuseOrder = () => {
    openModal(RefuseOrder)
  }
  const cancelOrder = () => {
    openModal(CancelOrder)
  }
  const fulfillOrder = () => {
    openModal(FulfillOrder, true)
  }
  const deliverOrder = () => {
    openModal(DeliverOrder, true)
  }
  const invoiceDelivery = () => {
    openModal(InvoiceDelivery)
  }

  const onClose = async (result: IModalResult<any>) => {
    if (result.isSuccess)
      await loadOrder(id)
  }

  const openModal = (Modal, hasWidth: boolean = false) => {
    open(
      Modal,
      {
        order,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: false,
        styleWindow: hasWidth ? { minWidth: '80%' } : {}
      }
    )
  }

  const loadOrder = async (orderId: string) => {
    try {
      isLoading = true
      order = await mediator.send(new GetOrderQuery(orderId))

      if (order.delivery?.status != DeliveryStatus.Pending) {
        totalWholeSalePrice = order.delivery.totalWholeSalePrice
        totalVatPrice = order.delivery.totalVatPrice
        totalOnSalePrice = order.delivery.totalOnSalePrice
      } else {
        totalWholeSalePrice = order.totalWholeSalePrice
        totalVatPrice = order.totalVatPrice
        totalOnSalePrice = order.totalOnSalePrice
      }

      products = order.lines.filter(l => l.kind == LineKind.Product)?.map(p => {
        const preparedProduct = order.delivery?.lines.filter(l => l.identifier == p.identifier && l.kind == LineKind.Product)[0]
        const adjustedProduct = order.delivery?.adjustments.filter(l => l.identifier == p.identifier && l.kind == LineKind.Product)[0]

        return {
          ...p,
          preparedQuantity: preparedProduct?.quantity ?? 0,
          deliveredQuantity: (preparedProduct?.quantity + adjustedProduct?.quantity) ?? 0,
          totalWholeSalePrice: (order.delivery?.status !== DeliveryStatus.Pending) ? (preparedProduct.totalWholeSalePrice ?? 0) + (adjustedProduct?.totalWholeSalePrice ?? 0) : p.totalWholeSalePrice,
          totalVatPrice: (order.delivery?.status !== DeliveryStatus.Pending) ? (preparedProduct.totalVatPrice ?? 0) + (adjustedProduct?.totalVatPrice ?? 0) : p.totalVatPrice,
          totalOnSalePrice: (order.delivery?.status !== DeliveryStatus.Pending) ? (preparedProduct.totalOnSalePrice ?? 0) + (adjustedProduct?.totalOnSalePrice ?? 0) : p.totalOnSalePrice
        }
      }) ?? []

      returnables = order.lines.filter(l => l.kind == LineKind.Returnable)?.map(p => {
        const preparedReturnable = order.delivery?.lines.filter(l => l.identifier == p.identifier && l.kind == LineKind.Returnable)[0]
        const adjustedReturnable = order.delivery?.adjustments.filter(l => l.identifier == p.identifier && l.kind == LineKind.Returnable)[0]

        return {
          ...p,
          quantity: (preparedReturnable?.quantity > 0 ? preparedReturnable.quantity : p.quantity) + (adjustedReturnable?.quantity ?? 0),
          totalWholeSalePrice: (order.delivery?.status !== DeliveryStatus.Pending) ? (preparedReturnable.totalWholeSalePrice ?? 0) + (adjustedReturnable?.totalWholeSalePrice ?? 0) : p.totalWholeSalePrice,
          totalVatPrice: (order.delivery?.status !== DeliveryStatus.Pending) ? (preparedReturnable.totalVatPrice ?? 0) + (adjustedReturnable?.totalVatPrice ?? 0) : p.totalVatPrice,
          totalOnSalePrice: (order.delivery?.status !== DeliveryStatus.Pending) ? (preparedReturnable.totalOnSalePrice ?? 0) + (adjustedReturnable?.totalOnSalePrice ?? 0) : p.totalOnSalePrice
        }
      }) ?? []

      returnedReturnables = order.delivery?.adjustments?.filter(a => a.quantity < 0 && a.kind == LineKind.ReturnedReturnable) ?? []

      title = `Commande ${order.code}`
      isLoading = false
    } catch (exc) {
      module.goBack()
    }
  }

  onMount(async () => {
    await loadOrder(id)
  })

  $: actions = [
    {
      name: 'Accepter',
      disabled: isLoading,
      visible: order?.canAcceptOrRefuse,
      color: 'success',
      action: () => acceptOrder()
    },
    {
      name: 'Refuser',
      disabled: isLoading,
      visible: order?.canAcceptOrRefuse,
      color: 'danger',
      action: () => refuseOrder()
    },
    {
      name: 'Annuler',
      disabled: isLoading,
      visible: order?.canCancel,
      color: 'warning',
      action: () => cancelOrder()
    },
    {
      name: 'Préparer',
      disabled: isLoading,
      visible: order?.canFulfill,
      color: 'accent',
      action: () => fulfillOrder()
    },
    {
      name: 'Livrer',
      disabled: isLoading,
      visible: order?.canComplete,
      color: 'accent',
      action: () => deliverOrder()
    },
    {
      name: 'Facturer',
      disabled: isLoading,
      visible: order?.canInvoice,
      color: 'accent',
      action: () => invoiceDelivery()
    }
  ]

  $: isLoading = true
</script>

<!-- routify:options title="Détails de la commande" -->
<!-- routify:options index=true -->

<PageHeader {title} {actions} />

<PageContent>
  <div class='flex'>
    {#if $authStore.account.profile.kind === ProfileKind.Customer}
      <div class='bg-white rounded py-4 px-6 mb-3 mr-2 flex-grow border-r'>
        <h3 class='mb-2'>Fournisseur</h3>
        <p class='font-medium'>{order?.supplier.name}</p>
        <p><a href='mailto:{order?.supplier.email}'>{order?.supplier.email}</a></p>
        <p><a href='tel:{order?.supplier.phone}'>{order?.supplier.phone}</a></p>
      </div>
    {:else}
      <div class='bg-white rounded py-4 px-6 mb-3 mr-2 flex-grow border-r'>
        <h3 class='mb-2'>Client</h3>
        <p class='font-medium'>{order?.customer.name}</p>
        <p><a href='mailto:{order?.customer.email}'>{order?.customer.email}</a></p>
        <p><a href='tel:{order?.customer.phone}'>{order?.customer.phone}</a></p>
      </div>
    {/if}
    <div class='bg-white rounded py-4 px-6 mb-3 mr-2 flex-grow border-r'>
      <h3 class='mb-2'>Statut</h3>
      <p>Commande {$authStore.account.profile.kind === ProfileKind.Customer ? 'créée' : 'reçue'}
        le {dateStr(order?.publishedOn, "dd/MM/yyyy")}</p>
      <p>
        {#if order?.status === OrderStatus.Pending && $authStore.account.profile.kind === ProfileKind.Supplier}
          Elle est en attente de votre acceptation
        {/if}
        {#if order?.status === OrderStatus.Pending && $authStore.account.profile.kind === ProfileKind.Customer}
          Elle est en attente d'acceptation du fournisseur
        {/if}
        {#if order?.status === OrderStatus.Accepted && $authStore.account.profile.kind === ProfileKind.Customer}
          Elle a été acceptée par le fournisseur, sa préparation est en cours
        {/if}
        {#if order?.status === OrderStatus.Accepted && $authStore.account.profile.kind === ProfileKind.Supplier}
          Elle est cours de préparation
        {/if}
        {#if order?.status === OrderStatus.Fulfilled}
          Elle est prête à être livrée depuis le {dateStr(order?.fulfilledOn, "dd/MM/yyyy")}
        {/if}
        {#if order?.status === OrderStatus.Completed}
          Elle a été livrée le {dateStr(order?.completedOn, "dd/MM/yyyy")}
        {/if}
        {#if order?.status === OrderStatus.Cancelled}
          Elle a été annulée le {dateStr(order?.abortedOn, "dd/MM/yyyy")}
        {/if}
        {#if order?.status === OrderStatus.Refused}
          Elle a été refusée le {dateStr(order?.abortedOn, "dd/MM/yyyy")}
        {/if}
      </p>
    </div>
    <div class='bg-white rounded py-4 px-6 mb-3 flex-grow'>
      <h3 class='mb-2'>Livraison prévue</h3>
      <p class='font-medium'>le {dateStr(order?.delivery.scheduledAt)}</p>
      <p>{order?.delivery.address.street}</p>
      {#if order?.delivery.address.complement}
        <p>{order?.delivery.address.complement}</p>
      {/if}
      <p>{order?.delivery.address.postcode} {order?.delivery.address.city}</p>
    </div>
  </div>
  {#if order?.status === OrderStatus.Cancelled || order?.status === OrderStatus.Refused}
    <div class='bg-white rounded py-4 px-6 mb-3 w-full'>
      <p class='font-medium'>Cette commande à été {orderStatus(order).toLowerCase()} pour la raison suivante: </p>
      <p>{order?.reason}</p>
    </div>
  {/if}
  {#if order?.status === OrderStatus.Completed && order?.delivery?.comments?.length > 0}
    <div class='bg-white rounded py-4 px-6 mb-3 w-full'>
      <p class='font-medium'>Un commentaire a été laissé pour la livraison correspondante: </p>
      <p>{order?.delivery.comments}</p>
    </div>
  {/if}
  <table>
    <thead>
    <tr>
      <th>Nom</th>
      <th class='text-right'>Prix unitaire (HT)</th>
      <th class='text-right'>TVA</th>
      <th class='text-right'>Quantité</th>
      <th class='text-right'>Prix total (TTC)</th>
    </tr>
    </thead>
    <tbody>
    {#if products?.length > 0 && returnables?.length > 0}
      <tr class='bg-gray-50 hover:bg-gray-50 cursor-default'>
        <th colspan='5' class='font-bold text-gray-600'>Produits</th>
      </tr>
    {/if}
    {#each products ?? [] as product}
      <tr class='cursor-default'>
        <td><p class='font-semibold'>{product.name}</p>
          <small>#{product.code}</small></td>
        <td use:formatInnerHtml={currency} class='text-right'>{product.unitPrice}</td>
        <td use:formatInnerHtml={percent} class='text-right'>{product.vat}</td>
        <td class='text-right'>
          {#if order.delivery?.status === DeliveryStatus.Delivered}
            <p>{product.quantity} commandés</p>
            <p>{product.preparedQuantity} préparés</p>
            <p>{product.deliveredQuantity} livrés</p>
          {:else if order.delivery?.status === DeliveryStatus.Scheduled}
            <p>{product.quantity} commandés</p>
            <p>{product.preparedQuantity} préparés</p>
          {:else}
            {product.quantity}
          {/if}
        </td>
        <td use:formatInnerHtml={currency} class='text-right'>{product.totalOnSalePrice}</td>
      </tr>
    {/each}
    {#if returnables?.length > 0}
      <tr class='bg-gray-50 hover:bg-gray-50 cursor-default'>
        <th colspan='5' class='font-bold text-gray-600'>
          Consignes {order.delivery?.status === DeliveryStatus.Delivered ? 'déposées' : ''}</th>
      </tr>
      {#each returnables ?? [] as returnable}
        <tr class='cursor-default'>
          <td class='font-semibold'>{returnable.name}</td>
          <td use:formatInnerHtml={currency} class='text-right'>{returnable.unitPrice}</td>
          <td use:formatInnerHtml={percent} class='text-right'>{returnable.vat}</td>
          <td
            class='text-right'>
            {returnable.quantity}
          </td>
          <td use:formatInnerHtml={currency} class='text-right'>{returnable.totalOnSalePrice}</td>
        </tr>
      {/each}
    {/if}
    {#if returnedReturnables?.length > 0}
      <tr class='bg-gray-50 hover:bg-gray-50 cursor-default'>
        <th colspan='5' class='font-bold text-gray-600'>Consignes récupérées</th>
      </tr>
      {#each returnedReturnables ?? [] as returnedReturnable}
        <tr class='cursor-default'>
          <td class='font-semibold'>{returnedReturnable.name}</td>
          <td use:formatInnerHtml={currency} class='text-right'>{returnedReturnable.unitPrice}</td>
          <td use:formatInnerHtml={percent} class='text-right'>{returnedReturnable.vat}</td>
          <td
            class='text-right'>{returnedReturnable.quantity}</td>
          <td use:formatInnerHtml={currency} class='text-right'>{returnedReturnable.totalOnSalePrice}</td>
        </tr>
      {/each}
    {/if}
    </tbody>
    <tfoot>
    <tr class='hover:bg-white cursor-default'>
      <td colspan='4' class='text-right font-semibold'>Total HT</td>
      <td class='font-medium text-right'>{currency(totalWholeSalePrice)}</td>
    </tr>
    <tr class='hover:bg-white cursor-default'>
      <td colspan='4' class='text-right font-semibold'>Total TVA</td>
      <td class='font-medium text-right'>{currency(totalVatPrice)}</td>
    </tr>
    <tr class='hover:bg-white cursor-default'>
      <td colspan='4' class='text-right font-semibold'>Total TTC</td>
      <td class='font-medium text-right'>{currency(totalOnSalePrice)}</td>
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
