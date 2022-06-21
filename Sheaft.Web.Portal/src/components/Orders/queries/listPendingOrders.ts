import type { Client, Components } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import { OrderStatus } from '$components/Orders/enums'

export class ListPendingOrdersQuery extends Request<Promise<Components.Schemas.OrderDto[]>> {
  constructor(public page: number, public take: number) {
    super();
  }
}

export class ListPendingOrdersHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: ListPendingOrdersQuery
  ): Promise<Components.Schemas.OrderDto[]> => {
    try {    
      const { data } = await this._client.ListOrders({...request, statuses: [OrderStatus.Pending]});
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
