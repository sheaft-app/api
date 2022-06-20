import type { GotoHelper } from "@roxi/routify";
import type { IAppModule } from "$components/module";
import type { Client } from "$types/api";
import { AppModule } from "$components/module";
import {
  CreateProductCommand,
  CreateProductHandler
} from "$components/Products/commands/createProduct";
import {
  UpdateProductCommand,
  UpdateProductHandler
} from "$components/Products/commands/updateProduct";
import {
  RemoveProductCommand,
  RemoveProductHandler
} from "$components/Products/commands/removeProduct";
import {
  GetProductHandler,
  GetProductQuery
} from "$components/Products/queries/getProduct";
import {
  ListProductsHandler,
  ListProductsQuery
} from "$components/Products/queries/listProducts";
import {
  ListReturnablesOptionsHandler,
  ListReturnablesOptionsQuery
} from "$components/Products/queries/listReturnablesOptions";
import type { IAuthStore } from '$components/Account/store'

export interface IProductModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToCreate(): void;
}

class ProductModule extends AppModule implements IProductModule {
  private _basePath: string = "/products";

  constructor(private _client: Client, private _authStore: IAuthStore) {
    super();
  }

  override registerHandlers = () => {
    this.registerHandler(CreateProductCommand, request =>
      new CreateProductHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(UpdateProductCommand, request =>
      new UpdateProductHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(RemoveProductCommand, request =>
      new RemoveProductHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(GetProductQuery, request =>
      new GetProductHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(ListProductsQuery, request =>
      new ListProductsHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(ListReturnablesOptionsQuery, request =>
      new ListReturnablesOptionsHandler(this._client, this._authStore).handle(request)
    );
  };

  goToCreate = (): void => {
    this.navigate(`${this._basePath}/create`);
  };

  goToDetails = (id: string): void => {
    this.navigate(`${this._basePath}/${id}`);
  };

  goToList = (): void => {
    this.navigate(this._basePath);
  };
}

let module: IProductModule | undefined;

export const getProductModule = (goto: GotoHelper): IProductModule => {
  if (module) {
    (<ProductModule>module).setGoto(goto);
    return module;
  }

  throw "product module was not initialized, call registerProductModule() in App.svelte";
};

export const registerProductModule = (client: Client, authStore:IAuthStore): IProductModule => {
  if (module) return module;

  module = new ProductModule(client, authStore);
  module.registerHandlers();
  return module;
};
