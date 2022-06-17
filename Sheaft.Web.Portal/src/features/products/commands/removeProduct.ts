import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class RemoveProductCommand extends Request<Promise<void>> {
  constructor(public id: string) {
    super();
  }
}

export class RemoveProductRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: RemoveProductCommand): Promise<void> => {
    try {
      await this._client.DeleteProduct(request.id);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
