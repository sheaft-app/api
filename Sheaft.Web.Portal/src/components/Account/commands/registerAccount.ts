import { Request } from "jimmy-js";
import type { Client } from "$types/api";

export class RegisterAccountCommand extends Request<Promise<void>> {
  constructor(
    public email: string,
    public password: string,
    public confirm: string,
    public firstname: string,
    public lastname: string
  ) {
    super();
  }
}

export class RegisterAccountHandler {
  constructor(private _client: Client) {}

  handle = async (request: RegisterAccountCommand): Promise<void> => {
    try {
      await this._client.RegisterAccount(null, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
