import type { Client } from "$features/api";
import { Request } from "jimmy-js";

export class ResetPasswordCommand extends Request<Promise<void>> {
  constructor(
    public resetToken: string | null | undefined,
    public password: string | null | undefined,
    public confirm: string | null | undefined
  ) {
    super();
  }
}

export class ResetPasswordHandler {
  constructor(private _client: Client) {}

  handle = async (request: ResetPasswordCommand): Promise<void> => {
    try {
      await this._client.ResetPassword(null, request);
      return Promise.resolve();
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
