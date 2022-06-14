import type { Client } from "$features/api";
import { Request } from "jimmy-js";
import type { IAuthStore } from "$components/Auth/types";

export class LoginUserCommand extends Request<Promise<void>> {
  constructor(
    public username: string | null | undefined,
    public password: string | null | undefined
  ) {
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
