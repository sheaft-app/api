<script lang='ts'>
  import { afterPageLoad, goto, page, params } from '@roxi/routify'
  import PageContent from '$components/Page/PageContent.svelte'
  import PageHeader from '$components/Page/PageHeader.svelte'
  import { dateDistance, dateStr } from '$utils/dates'
  import { currency } from '$utils/money'
  import { formatInnerHtml } from '$actions/format'
  import { getOrderModule } from '$components/Orders/module'
  import type { Components } from '$types/api'
  import { mediator } from '$components/mediator'
  import { authStore } from '$components/Account/store'
  import { ProfileKind } from '$components/Account/enums'
  import Radio from '$components/Radio/Radio.svelte'
  import { OrderStatus, OrderTab } from '$components/Orders/enums'
  import { ListPendingOrdersQuery } from '$components/Orders/queries/listPendingOrders'
  import { ListCompletedOrdersQuery } from '$components/Orders/queries/listCompletedOrders'
  import { ListActiveOrdersQuery } from '$components/Orders/queries/listActiveOrders'
  import { ListDraftOrdersQuery } from '$components/Orders/queries/listDraftOrders'
  import { getContext } from 'svelte'
  import CreateDraft from '$components/Orders/Modals/CreateDraft.svelte'
  import type { IModalResult } from '$components/Modal/modal'
  import { ListAbortedOrdersQuery } from '$components/Orders/queries/listAbortedOrders'
  import { orderStatus } from '$components/Orders/utils'

  export let pageNumber: number = 1,
    take: number = 10

  const module = getOrderModule($goto)
  const { open } = getContext('simple-modal')

  const profileKind = $authStore.account?.profile?.kind

  let isLoading = true
  let selectedTab: OrderTab
  let orders: Components.Schemas.OrderDto[] = []

  const getOrderTargetName = (order: Components.Schemas.OrderDto): string => {
    return profileKind == ProfileKind.Customer ? order.supplierName : order.customerName
  }

  const listOrders = async (tab): Promise<void> => {
    try {
      isLoading = true
      let query = null
      switch (tab) {
        case OrderTab.Draft:
          query = new ListDraftOrdersQuery(pageNumber, take)
          break
        case OrderTab.Sent:
        case OrderTab.Pending:
          query = new ListPendingOrdersQuery(pageNumber, take)
          break
        case OrderTab.Delivered:
          query = new ListCompletedOrdersQuery(pageNumber, take)
          break
        case OrderTab.Aborted:
          query = new ListAbortedOrdersQuery(pageNumber, take)
          break
        case OrderTab.InProgress:
        default:
          query = new ListActiveOrdersQuery(pageNumber, take)
          break
      }
      orders = await mediator.send(query)
      isLoading = false
    } catch (exc) {
      module.goToHome()
    }
  }

  const goTo = (order: Components.Schemas.OrderDto): void => {
    if (order.status === OrderStatus.Draft) {
      module.goToDraft(order.id)
      return
    }

    module.goToDetails(order.id)
  }

  const onClose = (result: IModalResult<string>): void => {
    if (!result.isSuccess)
      return

    module.goToDraft(result.value)
  }

  const openCreateModal = () => {
    open(
      CreateDraft,
      {
        onClose
      },
      {
        closeButton: false,
        closeOnEsc: true,
        closeOnOuterClick: true
      }
    )
  }

  const actions = [
    {
      name: 'Créer une commande',
      disabled: false,
      visible: profileKind == ProfileKind.Customer,
      color: 'accent',
      action: () => openCreateModal()
    }
  ]

  $: availableTabs = profileKind == ProfileKind.Customer
    ? [OrderTab.Draft, OrderTab.Sent, OrderTab.InProgress, OrderTab.Delivered, OrderTab.Aborted]
    : [OrderTab.Pending, OrderTab.InProgress, OrderTab.Delivered, OrderTab.Aborted]

  $afterPageLoad(async (page) => {
    selectedTab = <OrderTab>$params.tab ?? OrderTab.InProgress
    await listOrders(selectedTab)
  })

  $:if (selectedTab)
    module.goToList({ tab: selectedTab })

</script>

<!-- routify:options menu="Mes commandes" -->
<!-- routify:options title="Mes commandes" -->
<!-- routify:options index=true -->
<!-- routify:options default=true -->

<PageHeader title='{$page.title}' actions='{actions}' />

<PageContent isLoading='{isLoading}'>
  <Radio bind:value={selectedTab} values='{availableTabs}' />
  <div class='relative overflow-x-auto shadow-md sm:rounded-lg'>
    <table>
      <thead>
      <tr>
        {#if selectedTab !== OrderTab.Draft}
          <th>N°commande</th>
        {/if}
        <th>{profileKind === ProfileKind.Customer ? 'Fournisseur' : 'Client'}</th>
        {#if selectedTab !== OrderTab.Draft}
          <th>Total HT</th>
        {/if}
        {#if selectedTab === OrderTab.Sent}
          <th>Envoyée</th>
        {/if}
        {#if selectedTab === OrderTab.Pending}
          <th>Reçue</th>
        {/if}
        {#if selectedTab === OrderTab.Pending || selectedTab === OrderTab.Sent || selectedTab === OrderTab.InProgress}
          <th>Livraison le</th>
        {/if}
        {#if selectedTab === OrderTab.Draft}
          <th>Créé le</th>
        {/if}
        {#if selectedTab === OrderTab.Delivered}
          <th>Livrée le</th>
          <th>Facturation</th>
        {/if}
        {#if selectedTab === OrderTab.Aborted}
          <th>Avortée le</th>
        {/if}
        {#if selectedTab === OrderTab.Aborted || selectedTab === OrderTab.InProgress}
          <th>Statut</th>
        {/if}
        {#if selectedTab === OrderTab.Draft || selectedTab === OrderTab.InProgress}
          <th>Mise à jour</th>
        {/if}
      </tr>
      </thead>
      <tbody>
      {#each orders as order}
        <tr on:click='{() => goTo(order)}'>
          {#if selectedTab !== OrderTab.Draft}
            <th>{order.code}</th>
          {/if}
          <th>{getOrderTargetName(order)}</th>
          {#if selectedTab !== OrderTab.Draft}
            <td use:formatInnerHtml='{currency}'>{order.totalWholeSalePrice}</td>
          {/if}
          {#if selectedTab === OrderTab.Pending || selectedTab === OrderTab.Sent}
            <td use:formatInnerHtml='{dateDistance}'>{order.publishedOn}</td>
          {/if}
          {#if selectedTab === OrderTab.Pending || selectedTab === OrderTab.Sent || selectedTab === OrderTab.InProgress}
            <td>{dateStr(order.deliveryScheduledAt, "dd/MM/yyyy")}</td>
          {/if}
          {#if selectedTab === OrderTab.Draft}
            <td>{dateStr(order.createdOn, "dd/MM/yyyy")}</td>
          {/if}
          {#if selectedTab === OrderTab.Delivered}
            <td>{dateStr(order.completedOn, "dd/MM/yyyy")}</td>
            <td>{#if !order.invoiceId}
              <span
                class='rounded-full py-2 px-4 text-white bg-warning-400'>En attente</span>
              {:else}
              <span
              class='rounded-full py-2 px-4 text-white bg-success-400'>Facturée</span>
              {/if}</td>
          {/if}
          {#if selectedTab === OrderTab.Aborted}
            <td>{dateStr(order.abortedOn, "dd/MM/yyyy")}</td>
          {/if}
          {#if selectedTab === OrderTab.Aborted || selectedTab === OrderTab.InProgress}
            <td>
              <span
                class='rounded-full py-2 px-4 text-white'
                class:bg-danger-400={order.status === OrderStatus.Refused}
                class:bg-warning-400={order.status === OrderStatus.Accepted || order.status === OrderStatus.Cancelled}
                class:bg-success-400={order.status === OrderStatus.Fulfilled}
              >{orderStatus(order)}</span>
            </td>
          {/if}
          {#if selectedTab === OrderTab.Draft || selectedTab === OrderTab.InProgress}
            <td use:formatInnerHtml='{dateDistance}'>{order.updatedOn}</td>
          {/if}
        </tr>
      {/each}
      {#if orders?.length < 1}
        <tr>
          <td colspan='5' class='text-center'>Aucune commande</td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>
</PageContent>
