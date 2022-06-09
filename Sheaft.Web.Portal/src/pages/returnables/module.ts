import type { Client } from '$types/api'
import { AppModule } from '$services/module'
import type { IAppModule } from '$services/module'
import { mediator } from '$services/mediator'
import type { GotoHelper } from '@roxi/routify'
import { CreateReturnableHandler, CreateReturnableRequest } from '$commands/returnables/createReturnable'
import { UpdateReturnableRequest, UpdateReturnableRequestHandler } from '$commands/returnables/updateReturnable'
import { GetReturnableHandler, GetReturnableQuery } from '$queries/returnables/getReturnable'
import { ListReturnablesHandler, ListReturnablesQuery } from '$queries/returnables/listReturnables'

export interface IReturnableModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToCreate(): void;
}

class ReturnableModule extends AppModule implements IReturnableModule {
  private _basePath: string = '/returnables'

  constructor(private _client: Client) {
    super()
  }

  override register = () => {
    mediator.handle(
      CreateReturnableRequest,
      request => new CreateReturnableHandler(this._client).handle(request))

    mediator.handle(
      UpdateReturnableRequest,
      request => new UpdateReturnableRequestHandler(this._client).handle(request))

    mediator.handle(
      GetReturnableQuery,
      request => new GetReturnableHandler(this._client).handle(request))
    
    mediator.handle(
      ListReturnablesQuery,
      request => new ListReturnablesHandler(this._client).handle(request))
  }

  goToCreate = (): void => {
    this.navigate(`${this._basePath}/create`)
  }

  goToDetails = (id: string): void => {
    this.navigate(`${this._basePath}/${id}`)
  }

  goToList = (): void => {
    this.navigate(this._basePath)
  }
}

let module: IReturnableModule | null = null

export const getReturnableModule = (goto: GotoHelper): IReturnableModule => {
  if (module) {
    (<ReturnableModule>module).setGoto(goto)
    return module
  }

  throw 'returnable module was not initialized, call initReturnableModule()'
}

export const initReturnableModule = (client: Client): IReturnableModule => {
  if (module)
    return module

  module = new ReturnableModule(client)
  module.register()
  return module
}
