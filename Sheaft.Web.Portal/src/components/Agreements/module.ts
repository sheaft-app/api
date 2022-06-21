import type { GotoHelper } from '@roxi/routify'
import type { Client } from '$types/api'
import type { IAuthStore } from '$components/Account/store'
import { type IAppModule, AppModule } from '$components/module'
import {
  ListActiveAgreementsHandler,
  ListActiveAgreementsQuery
} from '$components/Agreements/queries/listActiveAgreements'
import {
  ListSentAgreementsHandler,
  ListSentAgreementsQuery
} from '$components/Agreements/queries/listSentAgreements'
import {
  ListReceivedAgreementsHandler,
  ListReceivedAgreementsQuery
} from '$components/Agreements/queries/listReceivedAgreements'
import {
  GetAgreementHandler,
  GetAgreementQuery
} from '$components/Agreements/queries/getAgreement'
import {
  RefuseAgreementCommand,
  RefuseAgreementHandler
} from '$components/Agreements/commands/refuseAgreement'
import {
  CancelAgreementCommand,
  CancelAgreementHandler
} from '$components/Agreements/commands/cancelAgreement'
import {
  AcceptCustomerAgreementCommand,
  AcceptCustomerAgreementHandler
} from '$components/Agreements/commands/acceptCustomerAgreement'
import {
  AcceptSupplierAgreementCommand,
  AcceptSupplierAgreementHandler
} from '$components/Agreements/commands/acceptSupplierAgreement'
import {
  RevokeAgreementCommand,
  RevokeAgreementHandler
} from '$components/Agreements/commands/revokeAgreement'
import {
  UpdateAgreementDeliveryCommand,
  UpdateAgreementDeliveryHandler
} from '$components/Agreements/commands/updateAgreement'

export interface IAgreementModule extends IAppModule {
  goToSearch(): void;
  goToSearchProfile(id: string): void;
  goToList(): void;
  goToDetails(id: string): void;
}

export class AgreementModule extends AppModule implements IAgreementModule {
  constructor(
    protected _client: Client,
    protected _authStore: IAuthStore,
    protected _basePath = '/agreements'
  ) {
    super()
  }

  override registerHandlers = () => {
    this.registerHandler(ListActiveAgreementsQuery, request =>
      new ListActiveAgreementsHandler(this._client).handle(request)
    )

    this.registerHandler(ListSentAgreementsQuery, request =>
      new ListSentAgreementsHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(ListReceivedAgreementsQuery, request =>
      new ListReceivedAgreementsHandler(this._client, this._authStore).handle(request)
    )

    this.registerHandler(GetAgreementQuery, request =>
      new GetAgreementHandler(this._client).handle(request)
    )

    this.registerHandler(AcceptCustomerAgreementCommand, request =>
      new AcceptCustomerAgreementHandler(this._client).handle(request)
    )

    this.registerHandler(AcceptSupplierAgreementCommand, request =>
      new AcceptSupplierAgreementHandler(this._client).handle(request)
    )

    this.registerHandler(RefuseAgreementCommand, request =>
      new RefuseAgreementHandler(this._client).handle(request)
    )

    this.registerHandler(CancelAgreementCommand, request =>
      new CancelAgreementHandler(this._client).handle(request)
    )

    this.registerHandler(RevokeAgreementCommand, request =>
      new RevokeAgreementHandler(this._client).handle(request)
    )

    this.registerHandler(UpdateAgreementDeliveryCommand, request =>
      new UpdateAgreementDeliveryHandler(this._client).handle(request)
    )
  }

  goToList(): void {
    this.navigate(`${this._basePath}`)
  }

  goToDetails(id: string): void {
    this.navigate(`${this._basePath}/${id}`)
  }

  goToSearch(): void {
    this.navigate(`${this._basePath}/search`)
  }

  goToSearchProfile(id: string): void {
    this.navigate(`${this._basePath}/search/${id}`)
  }
}

let module: IAgreementModule | undefined

export const getAgreementModule = (goto: GotoHelper): IAgreementModule => {
  if (module) {
    (<AgreementModule>module).setGoto(goto)
    return module
  }

  throw 'agreement module was not initialized, call registerAgreementModule() in App.svelte'
}

export const registerAgreementModule = (
  client: Client,
  authStore: IAuthStore
): IAgreementModule => {
  if (module) return module

  module = new AgreementModule(client, authStore)
  module.registerHandlers()
  return module
}
