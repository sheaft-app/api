import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import type { Components } from "$types/api";

export class GetAvailableSupplierQuery extends Request<Promise<Components.Schemas.AvailableSupplierDto>> {
  constructor(public id: string) {
    super();
  }
}

export class GetAvailableSupplierHandler {
  constructor(private _client: Client) {}

  handle = async (request: GetAvailableSupplierQuery): Promise<Components.Schemas.AvailableSupplierDto> => {
    try {
      const { data } = await this._client.GetAvailableSupplier(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
