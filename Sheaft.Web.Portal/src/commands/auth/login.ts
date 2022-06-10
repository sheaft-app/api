import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$stores/auth'

export class LoginRequest extends Request<Promise<void>> {
  constructor(
    public username: string | null | undefined, 
    public password: string | null | undefined) {
    super();
  }
}

export class LoginRequestHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: LoginRequest):Promise<void> => {
    try {
      const { data } = await this._client.LoginUser(null, request)
      this._authStore.setConnectedUser(data);
      return Promise.resolve()
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error : exc })
    }
  }
}
