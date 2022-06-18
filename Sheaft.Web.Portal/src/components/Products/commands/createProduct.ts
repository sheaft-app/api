import type { Client } from "$types/api";
import { Request } from "jimmy-js";

export class CreateProductCommand extends Request<Promise<string>> {
  constructor(
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

export class CreateProductHandler {
  constructor(private _client: Client) {}

  handle = async (request: CreateProductCommand): Promise<string> => {
    try {
      const { data } = await this._client.CreateProduct(null, request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
