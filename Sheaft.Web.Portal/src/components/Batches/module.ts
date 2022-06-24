import type { GotoHelper } from "@roxi/routify";
import type { Client } from "$types/api";
import { type IAppModule, AppModule } from '$components/module'
import {
  CreateBatchCommand,
  CreateBatchHandler
} from "$components/Batches/commands/createBatch";
import {
  UpdateBatchCommand,
  UpdateBatchHandler
} from "$components/Batches/commands/updateBatch";
import {
  RemoveBatchCommand,
  RemoveBatchHandler
} from "$components/Batches/commands/removeBatch";
import {
  GetBatchHandler,
  GetBatchQuery
} from "$components/Batches/queries/getBatch";
import {
  ListBatchesHandler,
  ListBatchesQuery
} from "$components/Batches/queries/listBatches";
import type { IAuthStore } from '$components/Account/store'

export interface IBatchModule extends IAppModule {
  goToList(): void;
  goToDetails(id: string): void;
  goToCreate(): void;
}

class BatchModule extends AppModule implements IBatchModule {
  private _basePath: string = "/batches";

  constructor(private _client: Client, private _authStore: IAuthStore) {
    super();
  }

  override registerHandlers = () => {
    this.registerHandler(CreateBatchCommand, request =>
      new CreateBatchHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(UpdateBatchCommand, request =>
      new UpdateBatchHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(RemoveBatchCommand, request =>
      new RemoveBatchHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(GetBatchQuery, request =>
      new GetBatchHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(ListBatchesQuery, request =>
      new ListBatchesHandler(this._client, this._authStore).handle(request)
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

let module: IBatchModule | undefined;

export const getBatchModule = (goto: GotoHelper): IBatchModule => {
  if (module) {
    (<BatchModule>module).setGoto(goto);
    return module;
  }

  throw "batch module was not initialized, call registerBatchModule() in App.svelte";
};

export const registerBatchModule = (client: Client, authStore:IAuthStore): IBatchModule => {
  if (module) return module;

  module = new BatchModule(client, authStore);
  module.registerHandlers();
  return module;
};
