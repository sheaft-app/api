import type { GotoHelper } from "@roxi/routify";
import type { IAppModule } from '$features/module'
import type { Client } from '$features/api'
import {
  GetAvailableCustomerHandler,
  GetAvailableCustomerQuery
} from '$features/customers/queries/getAvailableCustomer'
import {
  ListAvailableCustomersHandler,
  ListAvailableCustomersQuery
} from '$features/customers/queries/listAvailableCustomers'
import {
  ProposeAgreementToCustomerCommand,
  ProposeAgreementToCustomerHandler
} from '$features/customers/commands/proposeAgreementToCustomer'
import { AgreementModule } from '$features/agreements/module'
import type { IAuthStore } from '$components/Auth/types'

export interface ICustomerModule extends IAppModule {
  goToCustomers(): void;
  goToSearch(): void;
  goToCustomer(id: string): void;
}

class CustomerModule extends AgreementModule implements ICustomerModule {
  constructor(client: Client, authStore:IAuthStore) {
    super(client, authStore, "/customers");
  }

  override registerHandlers = () => {
    this.registerHandler(GetAvailableCustomerQuery, request =>
      new GetAvailableCustomerHandler(this._client).handle(request)
    );

    this.registerHandler(ListAvailableCustomersQuery, request =>
      new ListAvailableCustomersHandler(this._client).handle(request)
    );

    this.registerHandler(ProposeAgreementToCustomerCommand, request =>
      new ProposeAgreementToCustomerHandler(this._client).handle(request)
    );
  };

  goToCustomers = (): void => {
    this.navigate(this._basePath);
  };

  goToCustomer(id: string): void {
    this.navigate(`${this._basePath}/search/${id}`);
  }

  goToSearch(): void {
    this.navigate(`${this._basePath}/search`);
  }
}

let module: ICustomerModule | undefined;

export const getCustomerModule = (goto: GotoHelper): ICustomerModule => {
  if (module) {
    (<CustomerModule>module).setGoto(goto);
    return module;
  }

  throw "customer module was not initialized, call registerAgreementModule() in App.svelte";
};

export const registerCustomerModule = (client: Client, authStore:IAuthStore): ICustomerModule => {
  if (module) return module;

  module = new CustomerModule(client, authStore);
  module.registerHandlers();
  return module;
};
