import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import type { ProductQuantity } from '$components/Orders/types'

export class AcceptOrderCommand extends Request<Promise<void>> {
  constructor(public id: string, public newDeliveryDate?:string) {
    super()
  }
}

export class AcceptOrderHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: AcceptOrderCommand): Promise<void> => {
    try {
      await this._client.AcceptOrder(request.id, request)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
