import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class CreateReturnableCommand extends Request<Promise<string>> {
  constructor(
    public name: string | null | undefined,
    public unitPrice: number | undefined,
    public vat: number | undefined,
    public code?: string | null | undefined
  ) {
    super();
  }
}

export class CreateReturnableHandler {
  constructor(private _client: Client) {}

  handle = async (request: CreateReturnableCommand): Promise<string> => {
    try {
      const { data } = await this._client.CreateReturnable(null, request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
