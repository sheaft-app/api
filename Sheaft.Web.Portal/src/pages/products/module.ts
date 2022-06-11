import type { Client } from "$types/api";
import { AppModule } from "$services/module";
import type { IAppModule } from "$services/module";
import type { GotoHelper } from "@roxi/routify";
import {
  CreateProductHandler,
  CreateProductRequest
} from "$commands/products/createProduct";
import {
  UpdateProductRequest,
  UpdateProductRequestHandler
} from "$commands/products/updateProduct";
import { GetProductHandler, GetProductQuery } from "$queries/products/getProduct";
import { ListProductsHandler, ListProductsQuery } from "$queries/products/listProducts";
import {
  ListReturnablesOptionsHandler,
  ListReturnablesOptionsQuery
} from "$queries/products/listReturnablesOptions";

export interface IProductModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToCreate(): void;
}

class ProductModule extends AppModule implements IProductModule {
  private _basePath: string = "/products";

  constructor(private _client: Client) {
    super();
    this.registerHandlers();
  }

  override registerHandlers = () => {
    this.registerHandler(CreateProductRequest, request =>
      new CreateProductHandler(this._client).handle(request)
    );

    this.registerHandler(UpdateProductRequest, request =>
      new UpdateProductRequestHandler(this._client).handle(request)
    );

    this.registerHandler(GetProductQuery, request =>
      new GetProductHandler(this._client).handle(request)
    );

    this.registerHandler(ListProductsQuery, request =>
      new ListProductsHandler(this._client).handle(request)
    );

    this.registerHandler(ListReturnablesOptionsQuery, request =>
      new ListReturnablesOptionsHandler(this._client).handle(request)
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

let module: IProductModule | null | undefined;

export const getProductModule = (goto: GotoHelper): IProductModule => {
  if (module) {
    (<ProductModule>module).setGoto(goto);
    return module;
  }

  throw "product module was not initialized, call initProductModule()";
};

export const initProductModule = (client: Client): IProductModule => {
  if (module) return module;

  module = new ProductModule(client);
  return module;
};
