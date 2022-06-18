import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";

export class GetAvailableCustomerQuery extends Request<Promise<Components.Schemas.AvailableCustomerDto>> {
  constructor(public id: string) {
    super();
  }
}

export class GetAvailableCustomerHandler {
  constructor(private _client: Client) {}

  handle = async (request: GetAvailableCustomerQuery): Promise<Components.Schemas.AvailableCustomerDto> => {
    try {
      const { data } = await this._client.GetAvailableCustomer(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
