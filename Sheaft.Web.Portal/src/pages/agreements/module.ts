import type { Client } from "$types/api";
import type { IAppModule } from "$services/module";
import type { GotoHelper } from "@roxi/routify";
import { AppModule } from "$services/module";
import { ListAgreementsHandler, ListAgreementsQuery } from '$queries/agreements/listAgreements'
import { GetAgreementHandler, GetAgreementQuery } from '$queries/agreements/getAgreement'

export interface IAgreementModule extends IAppModule {
}

class AgreementModule extends AppModule implements IAgreementModule {
  private _basePath: string = "/agreements";

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
  };
}

let module: IAgreementModule | null | undefined;

export const getAgreementModule = (goto: GotoHelper): IAgreementModule => {
  if (module) {
    (<AgreementModule>module).setGoto(goto);
    return module;
  }

  throw "account module was not initialized, call registerAgreementModule() in App.svelte";
};

export const registerAgreementModule = (client: Client
): IAgreementModule => {
  if (module) return module;

  module = new AgreementModule(client);
  return module;
};
