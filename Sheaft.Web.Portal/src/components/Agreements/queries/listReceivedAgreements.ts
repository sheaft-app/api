import { Request } from "jimmy-js";
import type { Client, Components } from "$types/api";
import type { IAuthStore } from "$components/Account/store";

export class ListReceivedAgreementsQuery extends Request<
  Promise<Components.Schemas.AgreementDto[]>
> {
  constructor(public page: number, public take: number, public search?: string) {
    super();
  }
}

export class ListReceivedAgreementsHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {}

  handle = async (
    request: ListReceivedAgreementsQuery
  ): Promise<Components.Schemas.AgreementDto[]> => {
    try {
      const { data } = await this._client.ListReceivedAgreements(request);
      return Promise.resolve(data.items ?? []);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
