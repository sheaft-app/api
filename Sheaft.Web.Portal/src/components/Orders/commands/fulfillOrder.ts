import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import type { DeliveryLine } from '$components/Orders/types'

export class FulfillOrderCommand extends Request<Promise<void>> {
  constructor(public id: string, public deliveryLines: DeliveryLine[], public newDeliveryDate?:string) {
    super()
  }
}

export class FulfillOrderHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: FulfillOrderCommand): Promise<void> => {
    try {
      await this._client.FulfillOrder(request.id, request)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
