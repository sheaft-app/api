import { Request } from "jimmy-js";
import type { Client } from "$types/api";
import type { IAuthStore } from "$components/Account/store";

export class LoginUserCommand extends Request<Promise<void>> {
  constructor(public username: string, public password: string) {
    super();
  }
}

export class LoginUserHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {}

  handle = async (request: LoginUserCommand): Promise<void> => {
    try {
      const { data } = await this._client.LoginUser(null, request);
      this._authStore.setConnectedUser(data);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
