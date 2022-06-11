import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import type { Components } from "$types/api";

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
