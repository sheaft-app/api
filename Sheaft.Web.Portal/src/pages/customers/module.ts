import type { Client } from "$types/api";
import { AppModule } from "$services/module";
import type { IAppModule } from "$services/module";
import type { GotoHelper } from "@roxi/routify";
import { GetAvailableCustomerHandler, GetAvailableCustomerQuery } from '$queries/customers/getAvailableCustomer'
import { ListAvailableCustomersHandler, ListAvailableCustomersQuery } from '$queries/customers/listAvailableCustomers'
import {
  ProposeAgreementToCustomerHandler,
  ProposeAgreementToCustomerRequest
} from '$commands/customers/proposeAgreementToCustomer'

export interface ICustomerModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToCustomer(id: string): void;
  goToCustomers(): void;
}

class CustomerModule extends AppModule implements ICustomerModule {
  private _basePath: string = "/customers";

  constructor(private _client: Client) {
    super();
    this.registerHandlers();
  }

  override registerHandlers = () => {
    this.registerHandler(GetAvailableCustomerQuery, request =>
      new GetAvailableCustomerHandler(this._client).handle(request)
    );

    this.registerHandler(ListAvailableCustomersQuery, request =>
      new ListAvailableCustomersHandler(this._client).handle(request)
    );

    this.registerHandler(ProposeAgreementToCustomerRequest, request =>
      new ProposeAgreementToCustomerHandler(this._client).handle(request)
    );
  };
  
  goToDetails = (id: string): void => {
    this.navigate(`${this._basePath}/${id}`);
  };

  goToList = (): void => {
    this.navigate(this._basePath);
  };

  goToCustomer(id: string): void {
    this.navigate(`${this._basePath}/search/${id}`);
  }

  goToCustomers(): void {
    this.navigate(`${this._basePath}/search`);
  }
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
