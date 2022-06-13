import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { DayOfWeek } from '$enums/days'

export class ProposeAgreementToCustomerRequest extends Request<Promise<string>> {
  constructor(
    public id: string | undefined,
    public deliveryDays: DayOfWeek[] | undefined,
    public orderDelayInHoursBeforeDeliveryDay: number | undefined
  ) {
    super()
  }
}

export class ProposeAgreementToCustomerHandler {
  constructor(private _client: Client) {
  }

  handle = async (request: ProposeAgreementToCustomerRequest): Promise<string> => {
    try {
      const { data } = await this._client.ProposeAgreementToCustomer({ id: request.id }, {
        deliveryDays: request.deliveryDays,
        orderDelayInHoursBeforeDeliveryDay: request.orderDelayInHoursBeforeDeliveryDay
      })
      return Promise.resolve(data)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
