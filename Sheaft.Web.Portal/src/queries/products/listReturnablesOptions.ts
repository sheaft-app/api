import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import type { Components } from "$types/api";
import { currency } from "$utils/format";

export interface IReturnableOption {
  label: string;
  value: string;
}

export class ListReturnablesOptionsQuery extends Request<Promise<IReturnableOption[]>> {
  constructor() {
    super();
  }
}

export class ListReturnablesOptionsHandler {
  constructor(private _client: Client) {}

  handle = async (request: ListReturnablesOptionsQuery): Promise<IReturnableOption[]> => {
    try {
      const { data } = await this._client.ListReturnables(1, 1000);
      const options =
        data.items?.map(r => {
          return {
            label: r.name ? `${r.name} - ${currency(r.unitPrice)} HT` : "",
            value: r.id ?? ""
          };
        }) ?? [];

      return Promise.resolve(options);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
