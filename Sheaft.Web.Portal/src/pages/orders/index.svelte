<script lang='ts'>
  import { goto, page } from '@roxi/routify'
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
  import { ListAbortedOrdersQuery } from '$components/Orders/queries/listAbortedOrders'
  import { ListActiveOrdersQuery } from '$components/Orders/queries/listActiveOrders'
  import { ListDraftOrdersQuery } from '$components/Orders/queries/listDraftOrders'
  import { getContext } from 'svelte'
  import CreateDraft from '$components/Orders/Modals/CreateDraft.svelte'
  import type { IModalResult } from '$components/Modal/modal'

  export let pageNumber: number = 1,
    take: number = 10

  const module = getOrderModule($goto)
  const { open } = getContext('simple-modal')

  const profileKind = $authStore.account?.profile?.kind

  let isLoading = true
  let selectedTab: OrderTab = OrderTab.Active
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
        case OrderTab.Pending:
          query = new ListPendingOrdersQuery(pageNumber, take)
          break
        case OrderTab.Completed:
          query = new ListCompletedOrdersQuery(pageNumber, take)
          break
        case OrderTab.Aborted:
          query = new ListAbortedOrdersQuery(pageNumber, take)
          break
        case OrderTab.Active:
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
    if(!result.isSuccess)
      return;
    
    module.goToDraft(result.value);
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
    ? [OrderTab.Draft, OrderTab.Pending, OrderTab.Active, OrderTab.Completed, OrderTab.Aborted]
    : [OrderTab.Pending, OrderTab.Active, OrderTab.Completed, OrderTab.Aborted]

  $:listOrders(selectedTab)
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
        <th>{profileKind === ProfileKind.Customer ? 'Fournisseur' : 'Client'}</th>
        <th>Total HT</th>
        {#if selectedTab === OrderTab.Pending}
          <th>{profileKind === ProfileKind.Customer ? "Envoyée" : "Reçue"}</th>
        {/if}
        {#if selectedTab === OrderTab.Pending || selectedTab === OrderTab.Active}
          <th>Livraison le</th>
        {/if}
        {#if selectedTab === OrderTab.Draft}
          <th>Créé le</th>
        {/if}
        {#if selectedTab === OrderTab.Completed}
          <th>Terminée le</th>
        {/if}
        <th>Mise à jour</th>
      </tr>
      </thead>
      <tbody>
      {#each orders as order}
        <tr on:click='{() => goTo(order)}'>
          <th>{getOrderTargetName(order)}</th>
          <td use:formatInnerHtml='{currency}'>{order.totalWholeSalePrice}</td>
          {#if selectedTab === OrderTab.Pending}
            <td use:formatInnerHtml='{dateDistance}'>{order.publishedOn}</td>
          {/if}
          {#if selectedTab === OrderTab.Pending || selectedTab === OrderTab.Active}
            <td>{dateStr(order.deliveryScheduledAt, "dd/MM/yyyy")}</td>
          {/if}
          {#if selectedTab === OrderTab.Draft}
            <td>{dateStr(order.createdOn, "dd/MM/yyyy")}</td>
          {/if}
          {#if selectedTab === OrderTab.Completed}
            <td>{dateStr(order.completedOn, "dd/MM/yyyy")}</td>
          {/if}
          <td use:formatInnerHtml='{dateDistance}'>{order.updatedOn}</td>
        </tr>
      {/each}
      {#if orders?.length < 1}
        <tr>
          <td colspan='4' class='text-center'>Aucune commande</td>
        </tr>
      {/if}
      </tbody>
    </table>
  </div>
</PageContent>
