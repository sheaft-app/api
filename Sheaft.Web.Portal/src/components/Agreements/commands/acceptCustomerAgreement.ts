import { Request } from "jimmy-js";
import type { Client, Components } from "$types/api";
import type { DayOfWeek } from "$enums/days";

export class AcceptCustomerAgreementCommand extends Request<Promise<void>> {
  constructor(
    public id: string,
    public deliveryDays: DayOfWeek[],
    public orderDelayInHoursBeforeDeliveryDay: number
  ) {
    super();
  }
}

export class AcceptCustomerAgreementHandler {
  constructor(private _client: Client) {}

  handle = async (request: AcceptCustomerAgreementCommand): Promise<void> => {
    try {
      await this._client.AcceptCustomerAgreement(request.id, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
