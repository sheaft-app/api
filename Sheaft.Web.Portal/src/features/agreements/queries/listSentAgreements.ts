import type { Client, Components } from "$features/api";
import { Request } from "jimmy-js";
import type { IAuthStore } from '$components/Auth/types'

export class ListSentAgreementsQuery extends Request<Promise<Components.Schemas.AgreementDto[]>> {
  constructor(public page: number | undefined, public take: number | undefined, public search?: string |undefined) {
    super();
  }
}

export class ListSentAgreementsHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (
    request: ListSentAgreementsQuery
  ): Promise<Components.Schemas.AgreementDto[]> => {
    try {
      const { data } = await this._client.ListSentAgreements(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
