import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class RemoveReturnableCommand extends Request<Promise<void>> {
  constructor(public id: string) {
    super();
  }
}

export class RemoveReturnableHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: RemoveReturnableCommand): Promise<void> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      await this._client.DeleteReturnable({ returnableId: request.id, supplierId: profileId});
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
