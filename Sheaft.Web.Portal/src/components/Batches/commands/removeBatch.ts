import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class RemoveBatchCommand extends Request<Promise<void>> {
  constructor(public id: string) {
    super();
  }
}

export class RemoveBatchHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: RemoveBatchCommand): Promise<void> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      await this._client.DeleteBatch({ batchId: request.id, supplierId: profileId});
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
