import { Request } from "jimmy-js";
import type { Client, Components } from '$types/api'

export class GetAgreementQuery extends Request<Promise<Components.Schemas.AgreementDto>> {
  constructor(public id: string) {
    super();
  }
}

export class GetAgreementHandler {
  constructor(private _client: Client) {}

  handle = async (request: GetAgreementQuery): Promise<Components.Schemas.AgreementDto> => {
    try {
      const { data } = await this._client.GetAgreement(request);
      return Promise.resolve(data);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
