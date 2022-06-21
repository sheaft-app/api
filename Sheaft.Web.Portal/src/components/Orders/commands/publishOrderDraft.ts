import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import type { ProductQuantity } from '$components/Orders/types'

export class PublishOrderDraftCommand extends Request<Promise<void>> {
  constructor(public id: string,
              public deliveryDate: string,
              public products?: ProductQuantity[]) {
    super()
  }
}

export class PublishOrderDraftHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: PublishOrderDraftCommand): Promise<void> => {
    try {
      await this._client.PublishOrderDraft(request.id, request)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
