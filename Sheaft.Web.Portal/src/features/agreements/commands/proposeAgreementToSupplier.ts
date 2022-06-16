import type { Client } from '$features/api'
import { Request } from 'jimmy-js'
import type { DayOfWeek } from '$enums/days'

export class ProposeAgreementToSupplierCommand extends Request<Promise<string>> {
  constructor(
    public id: string | undefined
  ) {
    super()
  }
}

export class ProposeAgreementToSupplierHandler {
  constructor(private _client: Client) {
  }

  handle = async (request: ProposeAgreementToSupplierCommand): Promise<string> => {
    try {
      const { data } = await this._client.ProposeAgreementToSupplier({ id: request.id })
      return Promise.resolve(data)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
