import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'
import type { BatchDateKind } from '$components/Batches/enums'

export class UpdateBatchCommand extends Request<Promise<void>> {
  constructor(
    public id: string,
    public number: string,
    public dateKind: BatchDateKind,
    public expirationDate: string,
    public productionDate?: string
  ) {
    super();
  }
}

export class UpdateBatchHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: UpdateBatchCommand): Promise<void> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      await this._client.UpdateBatch({ batchId: request.id, supplierId: profileId}, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
