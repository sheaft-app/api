import type { Client, Components } from '$features/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$components/Auth/types'

export class ListReceivedAgreementsQuery extends Request<Promise<Components.Schemas.AgreementDto[]>> {
  constructor(public page: number | undefined, public take: number | undefined, public search?: string |undefined) {
    super();
  }
}

export class ListReceivedAgreementsHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

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
