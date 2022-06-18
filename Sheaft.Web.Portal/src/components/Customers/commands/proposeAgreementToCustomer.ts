import type { Client } from '$types/api'
import type { DayOfWeek } from '$enums/days'
import { Request } from 'jimmy-js'

export class ProposeAgreementToCustomerCommand extends Request<Promise<string>> {
  constructor(
    public id: string,
    public deliveryDays: DayOfWeek[],
    public orderDelayInHoursBeforeDeliveryDay: number
  ) {
    super()
  }
}

export class ProposeAgreementToCustomerHandler {
  constructor(private _client: Client) {
  }

  handle = async (request: ProposeAgreementToCustomerCommand): Promise<string> => {
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
