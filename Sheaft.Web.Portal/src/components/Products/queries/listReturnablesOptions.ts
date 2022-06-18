import type { Client } from "$types/api";
import type { ReturnableOption } from "$components/Products/types";
import { Request } from "jimmy-js";
import { currency } from "$utils/money";

export class ListReturnablesOptionsQuery extends Request<Promise<ReturnableOption[]>> {
  constructor() {
    super();
  }
}

export class ListReturnablesOptionsHandler {
  constructor(private _client: Client) {}

  handle = async (request: ListReturnablesOptionsQuery): Promise<ReturnableOption[]> => {
    try {
      const { data } = await this._client.ListReturnables(1, 1000);
      const options =
        data.items?.map(r => {
          return {
            label: r.name ? `${r.name} - ${currency(r.unitPrice)} HT` : "",
            value: r.id ?? ""
          };
        }) ?? <ReturnableOption[]>[];

      return Promise.resolve(options);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
