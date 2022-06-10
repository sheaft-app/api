import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$stores/auth'

export class ResetPasswordRequest extends Request<Promise<void>> {
  constructor(
    public resetToken: string | null | undefined, 
    public password: string | null | undefined,
    public confirm: string | null | undefined) {
    super();
  }
}

export class ResetPasswordRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: ResetPasswordRequest):Promise<void> => {
    try {
      await this._client.ResetPassword(null, request)
      return Promise.resolve()
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error : exc })
    }
  }
}
