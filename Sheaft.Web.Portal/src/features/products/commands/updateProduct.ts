import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class UpdateProductCommand extends Request<Promise<void>> {
  constructor(
    public id: string,
    public name: string | null | undefined,
    public unitPrice: number | undefined,
    public vat: number | undefined,
    public code?: string | null | undefined,
    public description?: string | null | undefined,
    public returnableId?: string | null | undefined
  ) {
    super();
  }
}

export class UpdateProductRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: UpdateProductCommand): Promise<void> => {
    try {
      await this._client.UpdateProduct(request.id, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
