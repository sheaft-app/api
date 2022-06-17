import type { Client } from '$features/api'
import { Request } from 'jimmy-js'
import type { DayOfWeek } from '$features/agreements/enums'

export class ProposeAgreementToCustomerCommand extends Request<Promise<string>> {
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
