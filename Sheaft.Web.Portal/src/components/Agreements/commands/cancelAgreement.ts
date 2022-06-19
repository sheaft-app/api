import { Request } from "jimmy-js";
import type { Client, Components } from "$types/api";

export class CancelAgreementCommand extends Request<Promise<void>> {
  constructor(public id: string, public reason?:string) {
    super();
  }
}

export class CancelAgreementHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: CancelAgreementCommand
  ): Promise<void> => {
    try {
      await this._client.CancelAgreement(request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
