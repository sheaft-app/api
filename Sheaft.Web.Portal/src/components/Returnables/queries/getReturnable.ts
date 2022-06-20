import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class GetReturnableQuery extends Request<
  Promise<Components.Schemas.ReturnableDto>
> {
  constructor(public id: string) {
    super();
  }
}

export class GetReturnableHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: GetReturnableQuery
  ): Promise<Components.Schemas.ReturnableDto> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      const { data } = await this._client.GetReturnable({ returnableId: request.id, supplierId: profileId});
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
