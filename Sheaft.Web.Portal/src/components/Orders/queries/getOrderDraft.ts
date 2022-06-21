import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class GetOrderDraftQuery extends Request<
  Promise<Components.Schemas.OrderDraftDto>
> {
  constructor(public orderId: string) {
    super();
  }
}

export class GetOrderDraftHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: GetOrderDraftQuery
  ): Promise<Components.Schemas.OrderDraftDto> => {
    try {
      const { data } = await this._client.GetOrderDraft(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
