import type { GotoHelper } from "@roxi/routify";
import type { IAppModule } from '$features/module'
import { AppModule } from '$features/module'
import { ListAgreementsHandler, ListAgreementsQuery } from '$features/agreements/queries/listAgreements'
import { GetAgreementHandler, GetAgreementQuery } from '$features/agreements/queries/getAgreement'
import { GetAvailableCustomerHandler, GetAvailableCustomerQuery } from '$features/agreements/queries/getAvailableCustomer'
import { ListAvailableCustomersHandler, ListAvailableCustomersQuery } from '$features/agreements/queries/listAvailableCustomers'
import {
  ProposeAgreementToCustomerHandler,
  ProposeAgreementToCustomerCommand
} from '$features/agreements/commands/proposeAgreementToCustomer'
import type { Client } from '$features/api'

export interface IAgreementModule extends IAppModule {
  goToDetails(id: string): void;
  goToCustomer(id: string): void;
  goToCustomers(): void;
  goToAvailableCustomers(): void;
}

class AgreementModule extends AppModule implements IAgreementModule {
  private _customerBasePath: string = "/customers";
  private _supplierBasePath: string = "/suppliers";

  constructor(private _client: Client) {
    super();
    this.registerHandlers();
  }

  override registerHandlers = () => {
    this.registerHandler(ListAgreementsQuery, request =>
      new ListAgreementsHandler(this._client).handle(request)
    );
    
    this.registerHandler(GetAgreementQuery, request =>
      new GetAgreementHandler(this._client).handle(request)
    );

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


  goToDetails = (id: string): void => {
    this.navigate(`${this._customerBasePath}/${id}`);
  };

  goToCustomers = (): void => {
    this.navigate(this._customerBasePath);
  };

  goToCustomer(id: string): void {
    this.navigate(`${this._customerBasePath}/search/${id}`);
  }

  goToAvailableCustomers(): void {
    this.navigate(`${this._customerBasePath}/search`);
  }
}

let module: IAgreementModule | undefined;

export const getAgreementModule = (goto: GotoHelper): IAgreementModule => {
  if (module) {
    (<AgreementModule>module).setGoto(goto);
    return module;
  }

  throw "agreement module was not initialized, call registerAgreementModule() in App.svelte";
};

export const registerAgreementModule = (client: Client
): IAgreementModule => {
  if (module) return module;

  module = new AgreementModule(client);
  return module;
};
