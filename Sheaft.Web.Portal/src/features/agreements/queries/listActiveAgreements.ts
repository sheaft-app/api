import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";

export class ListActiveAgreementsQuery extends Request<Promise<Components.Schemas.AgreementDto[]>> {
  constructor(public page: number | undefined, public take: number | undefined, public search?: string |undefined) {
    super();
  }
}

export class ListActiveAgreementsHandler {
  constructor(private _client: Client) {}

  handle = async (
    request: ListActiveAgreementsQuery
  ): Promise<Components.Schemas.AgreementDto[]> => {
    try {
      const { data } = await this._client.ListActiveAgreements(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
