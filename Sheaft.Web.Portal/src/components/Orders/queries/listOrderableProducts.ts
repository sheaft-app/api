import type { Client, Components } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import { OrderStatus } from '$components/Orders/enums'

export class ListSupplierOrderableProductsQuery extends Request<Promise<Components.Schemas.OrderableProductDto[]>> {
  constructor(public supplierId:string) {
    super();
  }
}

export class ListSupplierOrderableProductsHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: ListSupplierOrderableProductsQuery
  ): Promise<Components.Schemas.OrderableProductDto[]> => {
    try {    
      const { data } = await this._client.ListOrderableProducts({...request, page: 1, take: 100});
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
