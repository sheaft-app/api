import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import type { Components } from "$types/api";

export class ListAvailableCustomersQuery extends Request<Promise<Components.Schemas.AvailableCustomerDto[]>> {
  constructor(public page: number | undefined, public take: number | undefined) {
    super();
  }
}

export class ListAvailableCustomersHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: ListAvailableCustomersQuery
  ): Promise<Components.Schemas.AvailableCustomerDto[]> => {
    try {
      const { data } = await this._client.ListAvailableCustomers(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
