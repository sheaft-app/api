import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$stores/auth'

export class ForgotPasswordRequest extends Request<Promise<void>> {
  constructor(
    public email: string | null | undefined) {
    super();
  }
}

export class ForgotPasswordRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: ForgotPasswordRequest):Promise<void> => {
    try {
      await this._client.ForgotPassword(null, request)
      return Promise.resolve()
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error : exc })
    }
  }
}
