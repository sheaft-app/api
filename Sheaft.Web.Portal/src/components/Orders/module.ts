import type { GotoHelper } from '@roxi/routify'
import type { Client } from '$types/api'
import type { IAuthStore } from '$components/Account/store'
import { type IAppModule, AppModule } from '$components/module'
import {
  GetOrderHandler,
  GetOrderQuery
} from '$components/Orders/queries/getOrder'
import {
  CreateOrderDraftCommand,
  CreateOrderDraftHandler
} from '$components/Orders/commands/createOrderDraft'
import {
  UpdateOrderDraftProductsCommand,
  UpdateOrderDraftProductsHandler
} from '$components/Orders/commands/updateOrderDraftProducts'
import { PublishOrderDraftCommand, PublishOrderDraftHandler } from '$components/Orders/commands/publishOrderDraft'
import { ListDraftOrdersHandler, ListDraftOrdersQuery } from '$components/Orders/queries/listDraftOrders'
import { ListPendingOrdersHandler, ListPendingOrdersQuery } from '$components/Orders/queries/listPendingOrders'
import { ListActiveOrdersHandler, ListActiveOrdersQuery } from '$components/Orders/queries/listActiveOrders'
import { ListCompletedOrdersHandler, ListCompletedOrdersQuery } from '$components/Orders/queries/listCompletedOrders'
import { ListAbortedOrdersHandler, ListAbortedOrdersQuery } from '$components/Orders/queries/listAbortedOrders'
import {
  ListSupplierOrderableProductsHandler,
  ListSupplierOrderableProductsQuery
} from '$components/Orders/queries/listOrderableProducts'
import { GetOrderDraftHandler, GetOrderDraftQuery } from '$components/Orders/queries/getOrderDraft'

export interface IOrderModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToDraft(id: string): void;
  goToCreate(): void;
}

class OrderModule extends AppModule implements IOrderModule {
  private _basePath: string = '/orders'

  constructor(private _client: Client, private _authStore: IAuthStore) {
    super()
  }

  override registerHandlers = () => {
    this.registerHandler(GetOrderQuery, request =>
      new GetOrderHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(GetOrderDraftQuery, request =>
      new GetOrderDraftHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(ListDraftOrdersQuery, request =>
      new ListDraftOrdersHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(ListPendingOrdersQuery, request =>
      new ListPendingOrdersHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(ListActiveOrdersQuery, request =>
      new ListActiveOrdersHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(ListCompletedOrdersQuery, request =>
      new ListCompletedOrdersHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(ListAbortedOrdersQuery, request =>
      new ListAbortedOrdersHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(CreateOrderDraftCommand, request =>
      new CreateOrderDraftHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(UpdateOrderDraftProductsCommand, request =>
      new UpdateOrderDraftProductsHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(PublishOrderDraftCommand, request =>
      new PublishOrderDraftHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(ListSupplierOrderableProductsQuery, request =>
      new ListSupplierOrderableProductsHandler(this._client, this._authStore).handle(request)
    )
  }

  goToDetails = (id: string): void => {
    this.navigate(`${this._basePath}/${id}`)
  }

  goToDraft = (id: string): void => {
    this.navigate(`${this._basePath}/drafts/${id}`)
  }

  goToList = (): void => {
    this.navigate(this._basePath)
  }

  goToCreate = (): void => {
    this.navigate(`${this._basePath}/drafts/create`)
  }
}

let module: IOrderModule | undefined

export const getOrderModule = (goto: GotoHelper): IOrderModule => {
  if (module) {
    (<OrderModule>module).setGoto(goto)
    return module
  }

  throw 'order module was not initialized, call registerOrderModule() in App.svelte'
}

export const registerOrderModule = (client: Client, authStore: IAuthStore): IOrderModule => {
  if (module) return module

  module = new OrderModule(client, authStore)
  module.registerHandlers()
  return module
}
