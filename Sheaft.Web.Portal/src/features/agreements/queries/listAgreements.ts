import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class ListAgreementsQuery extends Request<Promise<Components.Schemas.AgreementDto[]>> {
  constructor(public page: number | undefined, public take: number | undefined) {
    super();
  }
}

export class ListAgreementsHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: ListAgreementsQuery
  ): Promise<Components.Schemas.AgreementDto[]> => {
    try {
      const { data } = await this._client.ListAgreements(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
