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
  import { DeliveryStatus, OrderLineKind, OrderStatus } from '$components/Orders/enums'
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

  export let id: string
  const module = getOrderModule($goto)
  const { open } = getContext('simple-modal')

  let title = $page.title
  let order: Components.Schemas.OrderDetailsDto = null

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
    openModal(FulfillOrder)
  }

  const onClose = async (result: IModalResult<any>) => {
    if (result.isSuccess)
      await loadOrder(id)
  }

  const openModal = Modal => {
    open(
      Modal,
      {
        order,
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: false
      }
    )
  }

  const loadOrder = async (orderId: string) => {
    try {
      isLoading = true
      order = await mediator.send(new GetOrderQuery(orderId))
      title = `Commande n°${order.code}`
      isLoading = false
    } catch (exc) {
      module.goToList()
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
    }
  ]

  $: isLoading = true

  $:returnables = order?.lines.filter(l => l.kind == OrderLineKind.Returnable)
  $:products = order?.lines.filter(l => l.kind == OrderLineKind.Product)
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
      <p><b>{orderStatus(order)}</b></p>
      <p>{$authStore.account.profile.kind === ProfileKind.Customer ? 'Créée' : 'Reçue'}
        le {dateStr(order?.publishedOn)}</p>
      {#if order?.status === OrderStatus.Fulfilled}
        <p>Préparée le {dateStr(order?.fulfilledOn)}</p>
      {/if}
    </div>
    <div class='bg-white rounded py-4 px-6 mb-3 flex-grow'>
      <h3 class='mb-2'>Livraison {order?.delivery.status !== DeliveryStatus.Delivered ? 'prévue' : ''}</h3>
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
          {#if product.preparedQuantity > 0 && product.preparedQuantity !== product.orderedQuantity}
            <p>{product.orderedQuantity} commandé(s)</p>
            <p>{product.preparedQuantity} préparé(s)</p>
          {:else}
            {product.orderedQuantity}
          {/if}
        </td>
        <td use:formatInnerHtml={currency} class='text-right'>{product.totalOnSalePrice}</td>
      </tr>
    {/each}
    {#if returnables?.length > 0}
      <tr class='bg-gray-50 hover:bg-gray-50 cursor-default'>
        <th colspan='5' class='font-bold text-gray-600'>Consignes</th>
      </tr>
      {#each returnables ?? [] as returnable}
        <tr class='cursor-default'>
          <td class='font-semibold'>{returnable.name}</td>
          <td use:formatInnerHtml={currency} class='text-right'>{returnable.unitPrice}</td>
          <td use:formatInnerHtml={percent} class='text-right'>{returnable.vat}</td>
          <td
            class='text-right'>{returnable.preparedQuantity > 0 ? returnable.preparedQuantity : returnable.orderedQuantity}</td>
          <td use:formatInnerHtml={currency} class='text-right'>{returnable.totalOnSalePrice}</td>
        </tr>
      {/each}
    {/if}
    </tbody>
    <tfoot>
    <tr class='hover:bg-white cursor-default'>
      <td colspan='4' class='text-right font-semibold'>Total HT</td>
      <td class='font-medium text-right'>{currency(order?.totalWholeSalePrice)}</td>
    </tr>
    <tr class='hover:bg-white cursor-default'>
      <td colspan='4' class='text-right font-semibold'>Total TVA</td>
      <td class='font-medium text-right'>{currency(order?.totalVatPrice)}</td>
    </tr>
    <tr class='hover:bg-white cursor-default'>
      <td colspan='4' class='text-right font-semibold'>Total TTC</td>
      <td class='font-medium text-right'>{currency(order?.totalOnSalePrice)}</td>
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
