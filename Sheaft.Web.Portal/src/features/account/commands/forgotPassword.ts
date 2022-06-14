import type { Client } from "$features/api";
import { Request } from "jimmy-js";

export class ForgotPasswordCommand extends Request<Promise<void>> {
  constructor(public email: string | null | undefined) {
    super();
  }
}

export class ForgotPasswordHandler {
  constructor(private _client: Client) {}

  handle = async (request: ForgotPasswordCommand): Promise<void> => {
    try {
      await this._client.ForgotPassword(null, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
