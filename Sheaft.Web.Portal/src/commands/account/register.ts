import type { Client } from '$types/api'
import { Request } from 'jimmy-js'

export class RegisterRequest extends Request<Promise<void>> {
  constructor(
    public email: string | null | undefined, 
    public password: string | null | undefined,
    public confirm: string | null | undefined,
    public firstname: string | null | undefined,
    public lastname: string | null | undefined) {
    super();
  }
}

export class RegisterRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: RegisterRequest):Promise<void> => {
    try {
      await this._client.RegisterAccount(null, request)
      return Promise.resolve()
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error : exc })
    }
  }
}
