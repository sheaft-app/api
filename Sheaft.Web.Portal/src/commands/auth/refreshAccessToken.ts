import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { IAuthStore } from '$stores/auth'

export class RefreshAccessTokenRequest extends Request<Promise<void>> {
  constructor(
    public token: string | null | undefined) {
    super();
  }
}

export class RefreshAccessTokenRequestHandler {
  constructor(private _client: Client, private _authStore:IAuthStore) {}

  handle = async (request: RefreshAccessTokenRequest):Promise<void> => {
    try {
      const { data } = await this._client.RefreshAccessToken(null, request)
      this._authStore.setConnectedUser(data);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error : exc })
    }
  }
}
