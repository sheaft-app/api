import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class GetProductQuery extends Request<
  Promise<Components.Schemas.ProductDetailsDto>
> {
  constructor(public id: string) {
    super();
  }
}

export class GetProductHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: GetProductQuery
  ): Promise<Components.Schemas.ProductDetailsDto> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      const { data } = await this._client.GetProduct({ productId: request.id, supplierId: profileId });
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
