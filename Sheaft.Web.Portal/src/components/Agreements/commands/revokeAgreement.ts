import { Request } from "jimmy-js";
import type { Client, Components } from "$types/api";

export class RevokeAgreementCommand extends Request<Promise<void>> {
  constructor(public id: string, public reason: string) {
    super();
  }
}

export class RevokeAgreementHandler {
  constructor(private _client: Client) {}

  handle = async (request: RevokeAgreementCommand): Promise<void> => {
    try {
      await this._client.RevokeAgreement(request.id, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
