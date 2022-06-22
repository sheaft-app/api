import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import type { ProductQuantity } from '$components/Orders/types'

export class RefuseOrderCommand extends Request<Promise<void>> {
  constructor(public id: string, public refusalReason:string) {
    super()
  }
}

export class RefuseOrderHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: RefuseOrderCommand): Promise<void> => {
    try {
      await this._client.RefuseOrder(request.id, request)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
