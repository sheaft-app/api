import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class GetReturnableQuery extends Request<
  Promise<Components.Schemas.ReturnableDto>
> {
  constructor(public id: string) {
    super();
  }
}

export class GetReturnableHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: GetReturnableQuery
  ): Promise<Components.Schemas.ReturnableDto> => {
    try {
      const { data } = await this._client.GetReturnable(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
