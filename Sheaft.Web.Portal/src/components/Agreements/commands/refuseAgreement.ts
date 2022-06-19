import { Request } from "jimmy-js";
import type { Client, Components } from "$types/api";

export class RefuseAgreementCommand extends Request<Promise<void>> {
  constructor(public id: string, public reason?:string) {
    super();
  }
}

export class RefuseAgreementHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: RefuseAgreementCommand
  ): Promise<void> => {
    try {
      await this._client.RefuseAgreement(request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
