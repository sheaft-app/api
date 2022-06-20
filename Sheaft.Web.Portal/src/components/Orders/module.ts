import type { GotoHelper } from "@roxi/routify";
import type { IAppModule } from "$components/module";
import type { Client } from "$types/api";
import { AppModule } from "$components/module";
import {
  GetOrderHandler,
  GetOrderQuery
} from "$components/Orders/queries/getOrder";
import {
  ListOrdersHandler,
  ListOrdersQuery
} from "$components/Orders/queries/listOrders";
import type { IAuthStore } from '$components/Account/store'
import { CreateOrderDraftCommand, CreateOrderDraftHandler } from '$components/Orders/commands/createOrderDraft'
import {
  UpdateOrderDraftProductsCommand,
  UpdateOrderDraftProductsHandler
} from '$components/Orders/commands/updateOrderDraftProducts'
import { PublishOrderDraftCommand, PublishOrderDraftHandler } from '$components/Orders/commands/publishOrderDraft'

export interface IOrderModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
}

class OrderModule extends AppModule implements IOrderModule {
  private _basePath: string = "/orders";

  constructor(private _client: Client, private _authStore: IAuthStore) {
    super();
  }

  override registerHandlers = () => {
    this.registerHandler(GetOrderQuery, request =>
      new GetOrderHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(ListOrdersQuery, request =>
      new ListOrdersHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(CreateOrderDraftCommand, request =>
      new CreateOrderDraftHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(UpdateOrderDraftProductsCommand, request =>
      new UpdateOrderDraftProductsHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(PublishOrderDraftCommand, request =>
      new PublishOrderDraftHandler(this._client, this._authStore).handle(request)
    );
  };

  goToDetails = (id: string): void => {
    this.navigate(`${this._basePath}/${id}`);
  };

  goToList = (): void => {
    this.navigate(this._basePath);
  };
}

let module: IOrderModule | undefined;

export const getOrderModule = (goto: GotoHelper): IOrderModule => {
  if (module) {
    (<OrderModule>module).setGoto(goto);
    return module;
  }

  throw "order module was not initialized, call registerOrderModule() in App.svelte";
};

export const registerOrderModule = (client: Client, authStore:IAuthStore): IOrderModule => {
  if (module) return module;

  module = new OrderModule(client, authStore);
  module.registerHandlers();
  return module;
};
