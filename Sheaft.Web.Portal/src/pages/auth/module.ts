import type { Client } from '$types/api'
import { AppModule } from '$services/module'
import type { IAppModule } from '$services/module'
import { mediator } from '$services/mediator'
import type { GotoHelper } from '@roxi/routify'
import { LoginRequest, LoginRequestHandler } from '$commands/auth/login'
import type { IAuthStore } from '$stores/auth'
import { RefreshAccessTokenRequest, RefreshAccessTokenRequestHandler } from '$commands/auth/refreshAccessToken'
import { RegisterRequest, RegisterRequestHandler } from '$commands/auth/register'
import { ForgotPasswordRequest, ForgotPasswordRequestHandler } from '$commands/auth/forgotPassword'
import { ResetPasswordRequest, ResetPasswordRequestHandler } from '$commands/auth/resetPassword'
import { LogoutRequest, LogoutRequestHandler } from '$commands/auth/logout'

export interface IAuthModule extends IAppModule {
  redirectIfRequired(redirectUrl?:string): void;
}

class AuthModule extends AppModule implements IAuthModule {
  private _basePath: string = '/auth'

  constructor(private _client: Client, private _authStore:IAuthStore) {
    super()
  }

  override register = () => {
    mediator.handle(
      LoginRequest,
      request => new LoginRequestHandler(this._client, this._authStore).handle(request))
    
    mediator.handle(
      LogoutRequest,
      request => new LogoutRequestHandler(this._client, this._authStore).handle(request))
    
    mediator.handle(
      RefreshAccessTokenRequest,
      request => new RefreshAccessTokenRequestHandler(this._client, this._authStore).handle(request))
    
    mediator.handle(
      RegisterRequest,
      request => new RegisterRequestHandler(this._client).handle(request))
    
    mediator.handle(
      ForgotPasswordRequest,
      request => new ForgotPasswordRequestHandler(this._client).handle(request))
    
    mediator.handle(
      ResetPasswordRequest,
      request => new ResetPasswordRequestHandler(this._client).handle(request))
  }

  redirectIfRequired = (returnUrl:string): void => {
    this.navigate(returnUrl?.length > 1 ? returnUrl : '/')
  }
}

let module: IAuthModule | null = null

export const getAuthModule = (goto: GotoHelper): IAuthModule => {
  if (module) {
    (<AuthModule>module).setGoto(goto)
    return module
  }

  throw 'auth module was not initialized, call initAuthModule()'
}

export const initAuthModule = (client: Client, authStore:IAuthStore): IAuthModule => {
  if (module)
    return module

  module = new AuthModule(client, authStore)
  module.register()
  return module
}
