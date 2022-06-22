import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import type { IAuthStore } from '$components/Account/store'

export class GetNextSupplierDeliveryDatesQuery extends Request<Promise<string[]>
> {
  constructor(public supplierId: string) {
    super();
  }
}

export class GetNextSupplierDeliveryDatesHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: GetNextSupplierDeliveryDatesQuery
  ): Promise<string[]> => {
    try {
      const { data } = await this._client.GetNextSupplierDeliveryDates(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
