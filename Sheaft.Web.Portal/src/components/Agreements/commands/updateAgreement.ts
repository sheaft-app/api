import { Request } from "jimmy-js";
import type { Client, Components } from "$types/api";
import type { DayOfWeek } from '$enums/days'

export class UpdateAgreementDeliveryCommand extends Request<Promise<void>> {
  constructor(public id: string,
              public deliveryDays: DayOfWeek[],
              public limitOrderHourOffset: number) {
    super();
  }
}

export class UpdateAgreementDeliveryHandler {
  constructor(private _client: Client) {}

  handle = async (request: UpdateAgreementDeliveryCommand): Promise<void> => {
    try {
      await this._client.UpdateAgreementDelivery(request.id, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
