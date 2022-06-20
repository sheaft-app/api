import type { GotoHelper } from "@roxi/routify";
import type { IAgreementModule } from "$components/Agreements/module";
import type { Client } from "$types/api";
import type { IAuthStore } from "$components/Account/store";
import { AgreementModule } from "$components/Agreements/module";
import {
  GetAvailableCustomerHandler,
  GetAvailableCustomerQuery
} from "$components/Customers/queries/getAvailableCustomer";
import {
  ListAvailableCustomersHandler,
  ListAvailableCustomersQuery
} from "$components/Customers/queries/listAvailableCustomers";
import {
  ProposeAgreementToCustomerCommand,
  ProposeAgreementToCustomerHandler
} from "$components/Customers/commands/proposeAgreementToCustomer";

export interface ICustomerModule extends IAgreementModule {
}

class CustomerModule extends AgreementModule implements ICustomerModule {
  constructor(client: Client, authStore: IAuthStore) {
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
}

let module: ICustomerModule | undefined;

export const getCustomerModule = (goto: GotoHelper): ICustomerModule => {
  if (module) {
    (<CustomerModule>module).setGoto(goto);
    return module;
  }

  throw "customer module was not initialized, call registerAgreementModule() in App.svelte";
};

export const registerCustomerModule = (
  client: Client,
  authStore: IAuthStore
): ICustomerModule => {
  if (module) return module;

  module = new CustomerModule(client, authStore);
  module.registerHandlers();
  return module;
};
