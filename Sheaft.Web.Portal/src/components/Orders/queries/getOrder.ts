import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'

export class GetOrderQuery extends Request<
  Promise<Components.Schemas.OrderDetailsDto>
> {
  constructor(public orderId: string) {
    super();
  }
}

export class GetOrderHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: GetOrderQuery
  ): Promise<Components.Schemas.OrderDetailsDto> => {
    try {
      const { data } = await this._client.GetOrder(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
