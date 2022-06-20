import type { Client } from '$types/api'
import { Request } from "jimmy-js";
import type { IAuthStore } from '$components/Account/store'
import type { ProductQuantity } from '$components/Orders/types'

export class UpdateOrderDraftProductsCommand extends Request<Promise<void>> {
  constructor(public id:string, 
              public products?: ProductQuantity[]) {
    super();
  }
}

export class UpdateOrderDraftProductsHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: UpdateOrderDraftProductsCommand): Promise<void> => {
    try {
      await this._client.UpdateOrderDraftProducts(request.id, request);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
