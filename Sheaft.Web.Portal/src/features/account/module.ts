import type { IAppModule } from "$features/module";
import type { GotoHelper } from "@roxi/routify";
import type { IAuthStore } from "$components/Auth/types";
import { AppModule } from "$features/module";
import { LoginUserCommand, LoginUserHandler } from "./commands/loginUser";
import {
  RefreshAccessTokenCommand,
  RefreshAccessTokenHandler
} from "./commands/refreshAccessToken";
import { RegisterAccountCommand, RegisterAccountHandler } from "./commands/registerAccount";
import {
  ForgotPasswordCommand,
  ForgotPasswordHandler
} from "./commands/forgotPassword";
import {
  ResetPasswordCommand,
  ResetPasswordHandler
} from "./commands/resetPassword";
import { LogoutUserCommand, LogoutUserHandler } from "./commands/logoutUser";
import {
  ConfigureAccountCommand,
  ConfigureAccountHandler
} from "./commands/configureAccount";
import type { Client } from '$features/api'

export interface IAccountModule extends IAppModule {
  redirectIfRequired(redirectUrl?: string): void;
}

class AccountModule extends AppModule implements IAccountModule {
  private _basePath: string = "/account";

  constructor(private _client: Client, private _authStore: IAuthStore) {
    super();
  }

  override registerHandlers = () => {
    this.registerHandler(LoginUserCommand, request =>
      new LoginUserHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(LogoutUserCommand, request =>
      new LogoutUserHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(RefreshAccessTokenCommand, request =>
      new RefreshAccessTokenHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(RegisterAccountCommand, request =>
      new RegisterAccountHandler(this._client).handle(request)
    );

    this.registerHandler(ForgotPasswordCommand, request =>
      new ForgotPasswordHandler(this._client).handle(request)
    );

    this.registerHandler(ResetPasswordCommand, request =>
      new ResetPasswordHandler(this._client).handle(request)
    );

    this.registerHandler(ConfigureAccountCommand, request =>
      new ConfigureAccountHandler(this._client).handle(request)
    );
  };

  redirectIfRequired = (returnUrl: string): void => {
    this.navigate(returnUrl?.length > 1 ? returnUrl : "/");
  };
}

let module: IAccountModule | undefined;

export const getAccountModule = (goto: GotoHelper): IAccountModule => {
  if (module) {
    (<AccountModule>module).setGoto(goto);
    return module;
  }

  throw "account module was not initialized, call registerAccountModule() in App.svelte";
};

export const registerAccountModule = (
  client: Client,
  authStore: IAuthStore
): IAccountModule => {
  if (module) return module;

  module = new AccountModule(client, authStore);
  module.registerHandlers();
  return module;
};
