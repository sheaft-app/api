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
import {
  GetAvailableSupplierHandler,
  GetAvailableSupplierQuery
} from '$features/agreements/queries/getAvailableSupplier'
import {
  ListAvailableSuppliersHandler,
  ListAvailableSuppliersQuery
} from '$features/agreements/queries/listAvailableSuppliers'
import {
  ProposeAgreementToSupplierCommand,
  ProposeAgreementToSupplierHandler
} from '$features/agreements/commands/proposeAgreementToSupplier'

export interface IAgreementModule extends IAppModule {
  goToDetails(id: string): void;
  goToCustomer(id: string): void;
  goToCustomers(): void;
  goToAvailableCustomers(): void;
  goToSupplier(id: string): void;
  goToSuppliers(): void;
  goToAvailableSuppliers(): void;
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

    this.registerHandler(GetAvailableSupplierQuery, request =>
      new GetAvailableSupplierHandler(this._client).handle(request)
    );

    this.registerHandler(ListAvailableSuppliersQuery, request =>
      new ListAvailableSuppliersHandler(this._client).handle(request)
    );

    this.registerHandler(ProposeAgreementToSupplierCommand, request =>
      new ProposeAgreementToSupplierHandler(this._client).handle(request)
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
  
  goToSuppliers = (): void => {
    this.navigate(this._supplierBasePath);
  };

  goToSupplier(id: string): void {
    this.navigate(`${this._supplierBasePath}/search/${id}`);
  }

  goToAvailableSuppliers(): void {
    this.navigate(`${this._supplierBasePath}/search`);
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
