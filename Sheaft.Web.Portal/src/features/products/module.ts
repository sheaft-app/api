import type { Client } from "$features/api";
import { AppModule } from "$features/module";
import type { IAppModule } from "$features/module";
import type { GotoHelper } from "@roxi/routify";
import {
  CreateProductHandler,
  CreateProductCommand
} from "$features/products/commands/createProduct";
import {
  UpdateProductCommand,
  UpdateProductRequestHandler
} from "$features/products/commands/updateProduct";
import { GetProductHandler, GetProductQuery } from "$features/products/queries/getProduct";
import { ListProductsHandler, ListProductsQuery } from "$features/products/queries/listProducts";
import {
  ListReturnablesOptionsHandler,
  ListReturnablesOptionsQuery
} from "$features/products/queries/listReturnablesOptions";
import { CreateReturnableCommand, CreateReturnableHandler } from '$features/products/commands/createReturnable'
import {
  UpdateReturnableCommand,
  UpdateReturnableHandler
} from '$features/products/commands/updateReturnable'
import { GetReturnableHandler, GetReturnableQuery } from '$features/products/queries/getReturnable'
import { ListReturnablesHandler, ListReturnablesQuery } from '$features/products/queries/listReturnables'
import { RemoveReturnableCommand, RemoveReturnableRequestHandler } from '$features/products/commands/removeReturnable'
import { RemoveProductCommand, RemoveProductRequestHandler } from '$features/products/commands/removeProduct'

export interface IProductModule extends IAppModule {
  goToProductList(): void;
  goToProductDetails(id: string): void;
  goToCreateProduct(): void;
  goToReturnableList(): void;
  goToReturnableDetails(id: string): void;
  goToCreateReturnable(): void;
}

class ProductModule extends AppModule implements IProductModule {
  private _productBasePath: string = "/products";
  private _returnableBasePath: string = "/returnables";

  constructor(private _client: Client) {
    super();
    this.registerHandlers();
  }

  override registerHandlers = () => {
    this.registerHandler(CreateProductCommand, request =>
      new CreateProductHandler(this._client).handle(request)
    );

    this.registerHandler(UpdateProductCommand, request =>
      new UpdateProductRequestHandler(this._client).handle(request)
    );

    this.registerHandler(RemoveProductCommand, request =>
      new RemoveProductRequestHandler(this._client).handle(request)
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
    
    this.registerHandler(CreateReturnableCommand, request =>
      new CreateReturnableHandler(this._client).handle(request)
    );

    this.registerHandler(UpdateReturnableCommand, request =>
      new UpdateReturnableHandler(this._client).handle(request)
    );
    
    this.registerHandler(RemoveReturnableCommand, request =>
      new RemoveReturnableRequestHandler(this._client).handle(request)
    );

    this.registerHandler(GetReturnableQuery, request =>
      new GetReturnableHandler(this._client).handle(request)
    );

    this.registerHandler(ListReturnablesQuery, request =>
      new ListReturnablesHandler(this._client).handle(request)
    );
  };

  goToCreateProduct = (): void => {
    this.navigate(`${this._productBasePath}/create`);
  };

  goToProductDetails = (id: string): void => {
    this.navigate(`${this._productBasePath}/${id}`);
  };

  goToProductList = (): void => {
    this.navigate(this._productBasePath);
  };

  goToCreateReturnable = (): void => {
    this.navigate(`${this._returnableBasePath}/create`);
  };

  goToReturnableDetails = (id: string): void => {
    this.navigate(`${this._returnableBasePath}/${id}`);
  };

  goToReturnableList = (): void => {
    this.navigate(this._returnableBasePath);
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

export const registerProductModule = (client: Client): IProductModule => {
  if (module) return module;

  module = new ProductModule(client);
  return module;
};
