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

export interface ICustomerModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToCreate(): void;
}

class CustomerModule extends AppModule implements ICustomerModule {
  private _basePath: string = "/customers";

  constructor(private _client: Client) {
    super();
    this.registerHandlers();
  }

  override registerHandlers = () => {
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

let module: ICustomerModule | null | undefined;

export const getCustomerModule = (goto: GotoHelper): ICustomerModule => {
  if (module) {
    (<CustomerModule>module).setGoto(goto);
    return module;
  }

  throw "customer module was not initialized, call registerCustomerModule()";
};

export const registerCustomerModule = (client: Client): ICustomerModule => {
  if (module) return module;

  module = new CustomerModule(client);
  return module;
};
