import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import type { IAuthStore } from '$components/Account/store'
import { get } from 'svelte/store'

export class ListProductsQuery extends Request<Promise<Components.Schemas.ProductDto[]>> {
  constructor(public page: number, public take: number) {
    super();
  }
}

export class ListProductsHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: ListProductsQuery
  ): Promise<Components.Schemas.ProductDto[]> => {
    try {
      const profileId = get(this._authStore).account.profile.id;      
      const { data } = await this._client.ListProducts({ ...request, supplierId: profileId });
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
