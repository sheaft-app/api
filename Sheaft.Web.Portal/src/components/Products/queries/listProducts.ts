import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";

export class ListProductsQuery extends Request<Promise<Components.Schemas.ProductDto[]>> {
  constructor(public page: number, public take: number) {
    super();
  }
}

export class ListProductsHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: ListProductsQuery
  ): Promise<Components.Schemas.ProductDto[]> => {
    try {
      const { data } = await this._client.ListProducts(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
