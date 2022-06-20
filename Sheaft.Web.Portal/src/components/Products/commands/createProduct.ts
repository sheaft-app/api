import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class CreateProductCommand extends Request<Promise<string>> {
  constructor(
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

export class CreateProductHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: CreateProductCommand): Promise<string> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      const { data } = await this._client.CreateProduct({ supplierId: profileId }, request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
