import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import type { IAuthStore } from "$stores/auth";
import { get } from "svelte/store";

export class RefreshAccessTokenRequest extends Request<Promise<void>> {
  constructor() {
    super();
  }
}

export class RefreshAccessTokenRequestHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {}

  handle = async (request: RefreshAccessTokenRequest): Promise<void> => {
    try {
      const { data } = await this._client.RefreshAccessToken(null, {
        token: get(this._authStore)?.tokens?.refreshToken
      });
      this._authStore.setConnectedUser(data);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
