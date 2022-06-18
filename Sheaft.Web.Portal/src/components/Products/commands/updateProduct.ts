import type { Client } from "$types/api";
import { Request } from "jimmy-js";

export class UpdateProductCommand extends Request<Promise<void>> {
  constructor(
    public id: string,
    public name: string,
    public unitPrice: number,
    public vat: number,
    public code?: string | null | undefined,
    public description?: string | null | undefined,
    public returnableId?: string | null | undefined
  ) {
    super();
  }
}

export class UpdateProductHandler {
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
