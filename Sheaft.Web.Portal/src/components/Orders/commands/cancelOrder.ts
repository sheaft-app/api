import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import type { ProductQuantity } from '$components/Orders/types'

export class CancelOrderCommand extends Request<Promise<void>> {
  constructor(public id: string, public cancellationReason:string) {
    super()
  }
}

export class CancelOrderHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: CancelOrderCommand): Promise<void> => {
    try {
      await this._client.CancelOrder(request.id, request)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
