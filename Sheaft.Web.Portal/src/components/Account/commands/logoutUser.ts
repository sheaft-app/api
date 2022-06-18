import { Request } from "jimmy-js";
import type { Client } from '$types/api'
import type { IAuthStore } from '$components/Account/store'

export class LogoutUserCommand extends Request<Promise<void>> {
  constructor() {
    super();
  }
}

export class LogoutUserHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {}

  handle = async (request: LogoutUserCommand): Promise<void> => {
    try {
      this._authStore.clearConnectedUser();
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
