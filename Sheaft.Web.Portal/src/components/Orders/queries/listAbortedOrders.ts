import type { Client, Components } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import { OrderStatus } from '$components/Orders/enums'

export class ListAbortedOrdersQuery extends Request<Promise<Components.Schemas.OrderDto[]>> {
  constructor(public page: number, public take: number) {
    super();
  }
}

export class ListAbortedOrdersHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: ListAbortedOrdersQuery
  ): Promise<Components.Schemas.OrderDto[]> => {
    try {    
      const { data } = await this._client.ListOrders(<any>{...request, "statuses[0]": OrderStatus.Refused, "statuses[1]": OrderStatus.Cancelled});
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
