import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class RemoveReturnableCommand extends Request<Promise<void>> {
  constructor(public id: string) {
    super();
  }
}

export class RemoveReturnableRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: RemoveReturnableCommand): Promise<void> => {
    try {
      await this._client.DeleteReturnable(request.id);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
