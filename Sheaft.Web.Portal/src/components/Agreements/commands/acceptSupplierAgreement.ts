import { Request } from "jimmy-js";
import type { Client, Components } from "$types/api";

export class AcceptSupplierAgreementCommand extends Request<Promise<void>> {
  constructor(public id: string) {
    super();
  }
}

export class AcceptSupplierAgreementHandler {
  constructor(private _client: Client) {}

  handle = async (request: AcceptSupplierAgreementCommand): Promise<void> => {
    try {
      await this._client.AcceptSupplierAgreement(request.id, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
