import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class CreateOrderDraftCommand extends Request<Promise<string>> {
  constructor(public supplierId:string) {
    super();
  }
}

export class CreateOrderDraftHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: CreateOrderDraftCommand): Promise<string> => {
    try {
      const { data } = await this._client.CreateOrderDraft(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
