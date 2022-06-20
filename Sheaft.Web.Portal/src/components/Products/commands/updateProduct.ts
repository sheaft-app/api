import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class UpdateProductCommand extends Request<Promise<void>> {
  constructor(
    public id: string,
    public name: string,
    public unitPrice: number,
    public vat: number,
    public code?: string | null | undefined,
    public description?: string | null | undefined,
    public returnableId?: string | null | undefined
  ) {
    super();
  }
}

export class UpdateProductHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: UpdateProductCommand): Promise<void> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      await this._client.UpdateProduct({ productId: request.id, supplierId: profileId}, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
