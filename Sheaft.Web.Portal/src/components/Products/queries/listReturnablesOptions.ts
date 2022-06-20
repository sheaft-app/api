import type { Client } from "$types/api";
import type { ReturnableOption } from "$components/Products/types";
import { Request } from "jimmy-js";
import { currency } from "$utils/money";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class ListReturnablesOptionsQuery extends Request<Promise<ReturnableOption[]>> {
  constructor() {
    super();
  }
}

export class ListReturnablesOptionsHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: ListReturnablesOptionsQuery): Promise<ReturnableOption[]> => {
    try {
      const profileId = get(this._authStore).account.profile.id;
      const { data } = await this._client.ListReturnables({ supplierId: profileId, page:1, take: 100 });
      const options =
        data.items?.map(r => {
          return {
            label: r.name ? `${r.name} - ${currency(r.unitPrice)} HT` : "",
            value: r.id ?? ""
          };
        }) ?? <ReturnableOption[]>[];

      return Promise.resolve(options);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
