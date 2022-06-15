﻿import { writable } from "svelte-local-storage-store";
import jwt_decode from "jwt-decode";
import { ProfileStatus } from '$enums/profile'
import { RefreshAccessTokenCommand } from "$features/account/commands/refreshAccessToken";
import type { Components } from '$features/api'
import { mediator } from '$features/mediator'
import type { IAuthState, IAuthStore, IUser } from '$components/Auth/types'

const store = (): IAuthStore => {
  let _timer: any = null;

  const _state: IAuthState = {
    isRegistered: false,
    isAuthenticated: false,
    user: null
  };

  const { subscribe, set, update } = writable<IAuthState>("auth", _state);

  const sub = subscribe((values) => {
    _state.isAuthenticated = values.isAuthenticated;
    _state.isRegistered = values.isRegistered;
    _state.user = values.user;
    _state.tokens = values.tokens;
  })

  const userIsInRoles = (roles?: Array<string>): boolean => {
    if (!_state.isAuthenticated) return false;

    if (!roles) return true;
    
    if(typeof roles == 'string' && (<string>roles).indexOf('[') > -1)
      roles = JSON.parse((<string>roles).replace(/'/g, "\""));

    if (!_state.user) return false;

    if (!_state.user?.roles) _state.user.roles = [];

    const userInRoles = roles.filter(r => _state.user?.roles.includes(r));
    return userInRoles.length > 0;
  };

  const clearConnectedUser = (): void => {
    set({ isAuthenticated: false, isRegistered: false });
  };

  const setConnectedUser = (token: Components.Schemas.TokenResponse): void => {
    updateStateStoreWithToken(token);
  };

  const getUserFromAccessToken = (accessToken: string): IUser => {
    if (!accessToken)
      return {
        roles: [],
        profile: {
          status: ProfileStatus.Anonymous
        }
      };

    const decoded = jwt_decode<any>(accessToken);

    return {
      id: decoded.sub,
      username:
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
      name: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
      firstname:
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"],
      lastname: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"],
      email:
        decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
      roles: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"] ?? [decoded["http://schemas.sheaft.com/ws/identity/claims/profile/kind"]],
      profile: {
        id: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/identifier"],
        kind: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/kind"],
        name: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/name"],
        status: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/status"]
      }
    };
  };

  const updateStateStoreWithToken = (
    response: Components.Schemas.TokenResponse
  ): void => {
    if (!response) return;

    update(state => {
      let user = getUserFromAccessToken(response.access_token ?? "");

      let expiresAt = new Date();
      expiresAt.setSeconds(expiresAt.getSeconds() + (response.expires_in ?? 0));

      state.tokens = {
        accessToken: response.access_token ?? "",
        refreshToken: response.refresh_token ?? "",
        expiresAt: expiresAt,
        tokenType: response.token_type ?? ""
      };
      
      state.isAuthenticated = true;
      _state.isAuthenticated = state.isAuthenticated;
      state.isRegistered = !!user?.id && user.profile?.status == ProfileStatus.Registered;
      _state.isRegistered = state.isRegistered;
      state.user = user;

      return state;
    });
  };

  const monitorUserAccessToken = async (): Promise<void> => {
    if (!_state.isAuthenticated || !_state.tokens?.expiresAt) {
      clearRefreshTokensHandler();
      return Promise.resolve();
    }

    const expiresAt = new Date(_state.tokens.expiresAt);
    const minRefreshDate = new Date(expiresAt.valueOf() - 5 * 60000);
    if (minRefreshDate > new Date()) return Promise.resolve();

    await refreshToken();

    configureRefreshTokensHandler();
    return Promise.resolve();
  };

  const configureRefreshTokensHandler = (): void => {
    _timer = setTimeout(monitorUserAccessToken, 60000);
  };

  const clearRefreshTokensHandler = (): void => {
    if (_timer) clearTimeout(_timer);
  };

  const refreshToken = async () => {
    if (!_state || !_state.tokens?.refreshToken) return;

    try {
      await mediator.send(new RefreshAccessTokenCommand());
    } catch (e) {
      console.error("An error occured while refreshing access token");
    }
  };

  return {
    subscribe,
    userIsInRoles,
    setConnectedUser,
    clearConnectedUser,
    startMonitorUserAccessToken: monitorUserAccessToken
  };
};

export const authStore = store();
