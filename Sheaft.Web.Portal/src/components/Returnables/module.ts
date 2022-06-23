﻿import type { GotoHelper } from "@roxi/routify";
import type { Client } from "$types/api";
import { type IAppModule, AppModule } from '$components/module'
import {
  CreateReturnableCommand,
  CreateReturnableHandler
} from "$components/Returnables/commands/createReturnable";
import {
  UpdateReturnableCommand,
  UpdateReturnableHandler
} from "$components/Returnables/commands/updateReturnable";
import {
  RemoveReturnableCommand,
  RemoveReturnableHandler
} from "$components/Returnables/commands/removeReturnable";
import {
  GetReturnableHandler,
  GetReturnableQuery
} from "$components/Returnables/queries/getReturnable";
import {
  ListReturnablesHandler,
  ListReturnablesQuery
} from "$components/Returnables/queries/listReturnables";
import type { IAuthStore } from '$components/Account/store'

export interface IReturnableModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToCreate(): void;
}

class ReturnableModule extends AppModule implements IReturnableModule {
  private _basePath: string = "/returnables";

  constructor(private _client: Client, private _authStore: IAuthStore) {
    super();
  }

  override registerHandlers = () => {
    this.registerHandler(CreateReturnableCommand, request =>
      new CreateReturnableHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(UpdateReturnableCommand, request =>
      new UpdateReturnableHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(RemoveReturnableCommand, request =>
      new RemoveReturnableHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(GetReturnableQuery, request =>
      new GetReturnableHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(ListReturnablesQuery, request =>
      new ListReturnablesHandler(this._client, this._authStore).handle(request)
    );
  };

  goToCreate = (): void => {
    this.navigate(`${this._basePath}/create`);
  };

  goToDetails = (id: string): void => {
    this.navigate(`${this._basePath}/${id}`);
  };

  goToList = (): void => {
    this.navigate(this._basePath);
  };
}

let module: IReturnableModule | undefined;

export const getReturnableModule = (goto: GotoHelper): IReturnableModule => {
  if (module) {
    (<ReturnableModule>module).setGoto(goto);
    return module;
  }

  throw "returnable module was not initialized, call registerReturnableModule() in App.svelte";
};

export const registerReturnableModule = (client: Client, authStore:IAuthStore): IReturnableModule => {
  if (module) return module;

  module = new ReturnableModule(client, authStore);
  module.registerHandlers();
  return module;
};