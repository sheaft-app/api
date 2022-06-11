import type { Client } from "$types/api";
import { Request } from "jimmy-js";

export class UpdateReturnableRequest extends Request<Promise<void>> {
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

export class UpdateReturnableRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: UpdateReturnableRequest): Promise<void> => {
    try {
      await this._client.UpdateReturnable(request.id, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
