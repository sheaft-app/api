import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Account/store'
import type { LineQuantity} from '$components/Orders/types'

export class InvoiceDeliveryCommand extends Request<Promise<void>> {
  constructor(public id: string) {
    super()
  }
}

export class InvoiceDeliveryHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: InvoiceDeliveryCommand): Promise<void> => {
    try {
      await this._client.CreateInvoiceForDelivery(request.id)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
