import { Request } from "jimmy-js";
import { get } from "svelte/store";
import type { Client } from "$types/api";
import type { IAuthStore } from "$components/Account/store";

export class RefreshAccessTokenCommand extends Request<Promise<void>> {
  constructor() {
    super();
  }
}

export class RefreshAccessTokenHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {}

  handle = async (request: RefreshAccessTokenCommand): Promise<void> => {
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
