import type { GotoHelper } from "@roxi/routify";
import type { IAppModule } from '$features/module'
import { AppModule } from '$features/module'
import { ListActiveAgreementsHandler, ListActiveAgreementsQuery } from '$features/agreements/queries/listActiveAgreements'
import { GetAgreementHandler, GetAgreementQuery } from '$features/agreements/queries/getAgreement'
import { ListSentAgreementsHandler, ListSentAgreementsQuery } from '$features/agreements/queries/listSentAgreements'
import {
  ListReceivedAgreementsHandler,
  ListReceivedAgreementsQuery
} from '$features/agreements/queries/listReceivedAgreements'
import type { IAuthStore } from '$components/Auth/types'
import type { Client } from '$features/api'

export interface IAgreementModule extends IAppModule {
  goToDetails(id:string):void;
}

export class AgreementModule extends AppModule implements IAgreementModule {
  constructor(protected _client: Client, protected _authStore:IAuthStore, protected _basePath = "/agreements") {
    super();
  }

  override registerHandlers = () => {
    this.registerHandler(ListActiveAgreementsQuery, request =>
      new ListActiveAgreementsHandler(this._client).handle(request)
    );
    this.registerHandler(ListSentAgreementsQuery, request =>
      new ListSentAgreementsHandler(this._client, this._authStore).handle(request)
    );
    this.registerHandler(ListReceivedAgreementsQuery, request =>
      new ListReceivedAgreementsHandler(this._client, this._authStore).handle(request)
    );
    
    this.registerHandler(GetAgreementQuery, request =>
      new GetAgreementHandler(this._client).handle(request)
    );
  };

  goToDetails(id: string): void {
    this.navigate(`${this._basePath}/${id}`);
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

export const registerAgreementModule = (client: Client, authStore:IAuthStore): IAgreementModule => {
  if (module) return module;

  module = new AgreementModule(client, authStore);
  module.registerHandlers();
  return module;
};
