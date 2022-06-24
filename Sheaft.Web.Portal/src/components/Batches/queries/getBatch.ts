import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class GetBatchQuery extends Request<
  Promise<Components.Schemas.BatchDto>
> {
  constructor(public id: string) {
    super();
  }
}

export class GetBatchHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: GetBatchQuery
  ): Promise<Components.Schemas.BatchDto> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      const { data } = await this._client.GetBatch({ batchId: request.id, supplierId: profileId});
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
