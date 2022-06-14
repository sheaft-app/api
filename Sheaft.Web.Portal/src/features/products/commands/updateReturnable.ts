import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class UpdateReturnableCommand extends Request<Promise<void>> {
  constructor(
    public id: string,
    public name: string | null | undefined,
    public unitPrice: number | undefined,
    public vat: number | undefined,
    public code?: string | null | undefined
  ) {
    super();
  }
}

export class UpdateReturnableHandler {
  constructor(private _client: Client) {}

  handle = async (request: UpdateReturnableCommand): Promise<void> => {
    try {
      await this._client.UpdateReturnable(request.id, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
