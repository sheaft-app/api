import type { Client } from "$types/api";
import { Request } from "jimmy-js";
import type { IAuthStore } from "$stores/auth";

export class LogoutRequest extends Request<Promise<void>> {
  constructor() {
    super();
  }
}

export class LogoutRequestHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {}

  handle = async (request: LogoutRequest): Promise<void> => {
    try {
      this._authStore.clearConnectedUser();
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
