import type { Client } from "$types/api";
import type { IAppModule } from "$services/module";
import type { GotoHelper } from "@roxi/routify";
import type { IAuthStore } from "$stores/auth";
import { AppModule } from "$services/module";
import { LoginRequest, LoginRequestHandler } from "$commands/account/login";
import {
  RefreshAccessTokenRequest,
  RefreshAccessTokenRequestHandler
} from "$commands/account/refreshAccessToken";
import { RegisterRequest, RegisterRequestHandler } from "$commands/account/register";
import {
  ForgotPasswordRequest,
  ForgotPasswordRequestHandler
} from "$commands/account/forgotPassword";
import {
  ResetPasswordRequest,
  ResetPasswordRequestHandler
} from "$commands/account/resetPassword";
import { LogoutRequest, LogoutRequestHandler } from "$commands/account/logout";
import {
  ConfigureAccountRequest,
  ConfigureAccountRequestHandler
} from "$commands/account/configureAccount";

export interface IAccountModule extends IAppModule {
  redirectIfRequired(redirectUrl?: string): void;
}

class AccountModule extends AppModule implements IAccountModule {
  private _basePath: string = "/account";

  constructor(private _client: Client, private _authStore: IAuthStore) {
    super();
    this.registerHandlers();
  }

  override registerHandlers = () => {
    this.registerHandler(LoginRequest, request =>
      new LoginRequestHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(LogoutRequest, request =>
      new LogoutRequestHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(RefreshAccessTokenRequest, request =>
      new RefreshAccessTokenRequestHandler(this._client, this._authStore).handle(request)
    );

    this.registerHandler(RegisterRequest, request =>
      new RegisterRequestHandler(this._client).handle(request)
    );

    this.registerHandler(ForgotPasswordRequest, request =>
      new ForgotPasswordRequestHandler(this._client).handle(request)
    );

    this.registerHandler(ResetPasswordRequest, request =>
      new ResetPasswordRequestHandler(this._client).handle(request)
    );

    this.registerHandler(ConfigureAccountRequest, request =>
      new ConfigureAccountRequestHandler(this._client).handle(request)
    );
  };

  redirectIfRequired = (returnUrl: string): void => {
    this.navigate(returnUrl?.length > 1 ? returnUrl : "/");
  };
}

let module: IAccountModule | null | undefined;

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
  return module;
};
