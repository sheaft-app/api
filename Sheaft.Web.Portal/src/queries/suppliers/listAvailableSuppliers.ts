import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import type { Components } from "$types/api";

export class ListAvailableSuppliersQuery extends Request<Promise<Components.Schemas.AvailableSupplierDto[]>> {
  constructor(public page: number | undefined, public take: number | undefined) {
    super();
  }
}

export class ListAvailableSuppliersHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: ListAvailableSuppliersQuery
  ): Promise<Components.Schemas.AvailableSupplierDto[]> => {
    try {
      const { data } = await this._client.ListAvailableSuppliers(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
