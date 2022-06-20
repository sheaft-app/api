import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class ListReturnablesQuery extends Request<
  Promise<Components.Schemas.ReturnableDto[]>
> {
  constructor(public page: number, public take: number) {
    super();
  }
}

export class ListReturnablesHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: ListReturnablesQuery
  ): Promise<Components.Schemas.ReturnableDto[]> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      const { data } = await this._client.ListReturnables({ ...request, supplierId: profileId});
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
