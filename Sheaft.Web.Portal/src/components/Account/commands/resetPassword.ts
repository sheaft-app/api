import { Request } from "jimmy-js";
import type { Client } from '$types/api'

export class ResetPasswordCommand extends Request<Promise<void>> {
  constructor(
    public resetToken: string,
    public password: string,
    public confirm: string
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
