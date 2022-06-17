import type { GotoHelper } from "@roxi/routify";
import type { IAppModule } from '$features/module'
import type { Client } from '$features/api'
import {
  GetAvailableSupplierHandler,
  GetAvailableSupplierQuery
} from '$features/suppliers/queries/getAvailableSupplier'
import {
  ListAvailableSuppliersHandler,
  ListAvailableSuppliersQuery
} from '$features/suppliers/queries/listAvailableSuppliers'
import {
  ProposeAgreementToSupplierCommand,
  ProposeAgreementToSupplierHandler
} from '$features/suppliers/commands/proposeAgreementToSupplier'
import { AgreementModule } from '$features/agreements/module'
import type { IAuthStore } from '$components/Auth/types'

export interface ISupplierModule extends IAppModule {
  goToSupplier(id: string): void;
  goToSuppliers(): void;
  goToSearch(): void;
}

class SupplierModule extends AgreementModule implements ISupplierModule {
  constructor(client: Client, authStore:IAuthStore) {
    super(client, authStore, "/suppliers");
  }

  override registerHandlers = () => {
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
  
  goToSuppliers = (): void => {
    this.navigate(this._basePath);
  };

  goToSupplier(id: string): void {
    this.navigate(`${this._basePath}/search/${id}`);
  }

  goToSearch(): void {
    this.navigate(`${this._basePath}/search`);
  }
}

let module: ISupplierModule | undefined;

export const getSupplierModule = (goto: GotoHelper): ISupplierModule => {
  if (module) {
    (<SupplierModule>module).setGoto(goto);
    return module;
  }

  throw "supplier module was not initialized, call registerSupplierModule() in App.svelte";
};

export const registerSupplierModule = (client: Client, authStore:IAuthStore): ISupplierModule => {
  if (module) return module;

  module = new SupplierModule(client, authStore);
  module.registerHandlers();
  return module;
};
