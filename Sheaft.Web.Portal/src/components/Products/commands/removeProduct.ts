import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class RemoveProductCommand extends Request<Promise<void>> {
  constructor(public id: string) {
    super();
  }
}

export class RemoveProductHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: RemoveProductCommand): Promise<void> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      await this._client.DeleteProduct({ productId: request.id, supplierId: profileId});
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
