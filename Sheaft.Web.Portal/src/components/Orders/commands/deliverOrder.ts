import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import type { LineQuantity} from '$components/Orders/types'

export class DeliverOrderCommand extends Request<Promise<void>> {
  constructor(public id: string, public productsAdjustments: LineQuantity[], public returnedReturnables: LineQuantity[], public comments?:string) {
    super()
  }
}

export class DeliverOrderHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: DeliverOrderCommand): Promise<void> => {
    try {
      await this._client.DeliverOrder(request.id, request)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
