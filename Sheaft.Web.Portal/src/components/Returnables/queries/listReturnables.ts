import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";

export class ListReturnablesQuery extends Request<
  Promise<Components.Schemas.ReturnableDto[]>
> {
  constructor(public page: number, public take: number) {
    super();
  }
}

export class ListReturnablesHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: ListReturnablesQuery
  ): Promise<Components.Schemas.ReturnableDto[]> => {
    try {
      const { data } = await this._client.ListReturnables(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
