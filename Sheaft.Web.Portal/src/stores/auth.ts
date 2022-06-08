import { writable } from "svelte-local-storage-store";
import { derived, get } from "svelte/store";
import jwt_decode from "jwt-decode";
import { ProfileKind, ProfileStatus } from "$enums/profile";
import type {
  IRegisterAccount,
  IConfigureCustomer,
  IConfigureSupplier,
  IUser
} from "$types/auth";
import { api } from '$configs/axios'
import type { Client } from '$types/api'

let timer = null;
let initialized = false;

const userStore = writable("auth", {
  tokens: { accessToken: "", refreshToken: "", expiresAt: null, tokenType: "" },
  isAuthenticated: false,
  account: {
    id: null,
    username: null,
    name: null,
    firstname: null,
    lastname: null,
    email: null,
    roles: [],
    profile: {
      id: null,
      kind: null,
      name: null,
      status: ProfileStatus.Anonymous
    }
  }
});

export const account = derived([userStore], ([$userStore]) => $userStore.account);
export const tokens = derived([userStore], ([$userStore]) => $userStore.tokens);
export const isAuthenticated = derived(
  [userStore],
  ([$userStore]) => $userStore && $userStore.isAuthenticated
);
export const isRegistered = derived(
  [userStore],
  ([$userStore]) => $userStore &&
    $userStore.isAuthenticated &&
    $userStore.account.profile.status == ProfileStatus.Registered
);

export const login = async (username: string, password: string): Promise<boolean> => {
  try {
    const client = await api.getClient<Client>();
    const result = await client.LoginUser(null, { username, password });

    userStore.update(as => {
      as.isAuthenticated = true;
      as.tokens = getTokens(result.data);
      as.account = getUserFromAccessToken(as.tokens.accessToken);
      return as;
    });

    configureRefreshTokensHandler();

    return Promise.resolve(true);
  } catch (e) {
    return Promise.reject(false);
  }
};

export const logout = (): boolean => {
  try {
    userStore.update(as => {
      as.isAuthenticated = false;
      as.tokens = getTokens();
      as.account = getUserFromAccessToken();
      return as;
    });

    clearRefreshTokensHandler();

    return true;
  } catch (e) {
    return false;
  }
};

export const forgot = async (username: string): Promise<boolean> => {
  try {
    const client = await api.getClient<Client>();
    await client.ForgotPassword(null, { email: username });
    return Promise.resolve(true);
  } catch (e) {
    return Promise.reject(false);
  }
};

export const reset = async (
  code: string,
  newPassword: string,
  confirmPassword: string
): Promise<boolean> => {
  try {
    const client = await api.getClient<Client>();
    await client.ResetPassword(null, {
      resetToken: code,
      password: newPassword,
      confirm: confirmPassword
    });
    return Promise.resolve(true);
  } catch (e) {
    return Promise.reject(false);
  }
};

export const register = async (account: IRegisterAccount): Promise<boolean> => {
  try {
    const client = await api.getClient<Client>();
    await client.RegisterAccount(null, account);

    return Promise.resolve(true);
  } catch (e) {
    return Promise.reject(false);
  }
};

export const configureSupplier = async (
  account: IConfigureSupplier
): Promise<boolean> => {
  return configure(account, ProfileKind.Supplier);
};

export const configureCustomer = async (
  account: IConfigureCustomer
): Promise<boolean> => {
  return configure(account, ProfileKind.Customer);
};

const configure = async (
  account: any,
  type: ProfileKind
): Promise<boolean> => {
  try {
    const client = await api.getClient<Client>();
    if(type == ProfileKind.Customer)
      await client.ConfigureAccountAsCustomer(null, account);
    else
      await client.ConfigureAccountAsSupplier(null, account);  

    return Promise.resolve(true);
  } catch (e) {
    return Promise.reject(false);
  }
};

export const refreshToken = async () => {
  const user = get(userStore);
  if(!user)
    return;
  
  try {
    const client = await api.getClient<Client>();
    const result = await client.RefreshAccessToken(null, {
      token: user.tokens.refreshToken,
    });

    userStore.update(as => {
      as.tokens = getTokens(result.data);
      as.account = getUserFromAccessToken(as.tokens.accessToken);
      return as;
    });
  } catch (e) {
    console.error("An error occured while refreshing access token");
  }
};

export const userIsInRoles = (roles?: Array<string>): boolean => {
  if (!roles) return true;

  const user = get(userStore);
  if(!user)
    return false;
  
  if (!user.account.roles) user.account.roles = [];

  const userInRoles = roles.filter(r => user.account.roles.includes(r));
  return userInRoles.length > 0;
};

const getUserFromAccessToken = (access_token?: string) => {
  const user: IUser = {
    id: null,
    username: null,
    name: null,
    firstname: null,
    lastname: null,
    email: null,
    roles: [],
    profile: {
      id: null,
      kind: null,
      name: null,
      status: ProfileStatus.Anonymous
    }
  };
  if (!access_token) return user;

  const decoded = jwt_decode<any>(access_token);

  user.id = decoded.sub;
  user.username =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
  user.name = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
  user.firstname =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"];
  user.lastname =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname"];
  user.email =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
  user.roles = JSON.parse(
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"] ?? "[]"
  );
  user.profile = {
    id: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/identifier"],
    kind: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/kind"],
    name: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/name"],
    status: decoded["http://schemas.sheaft.com/ws/identity/claims/profile/status"]
  };

  return user;
};

const getTokens = (data?) => {
  const tokens = {
    accessToken: "",
    refreshToken: "",
    expiresAt: null,
    tokenType: ""
  };

  if (!data) return tokens;

  let expiresAt = new Date();
  expiresAt.setSeconds(expiresAt.getSeconds() + data.expires_in);

  tokens.accessToken = data.access_token;
  tokens.refreshToken = data.refresh_token;
  tokens.expiresAt = expiresAt;
  tokens.tokenType = data.token_type;

  return tokens;
};

const refreshTokensIfRequired = async () => {
  let user = get(userStore);
  if(!user)
    return;
  
  if (!user.isAuthenticated) {
    clearRefreshTokensHandler();
    return;
  }

  const expiresAt = new Date(user.tokens.expiresAt);
  const minRefreshDate = new Date(expiresAt.valueOf() - 5 * 60000);
  if (minRefreshDate > new Date()) return;

  await refreshToken();

  configureRefreshTokensHandler();
};

const configureRefreshTokensHandler = () => {
  timer = setTimeout(refreshTokensIfRequired, 60000);
};

const clearRefreshTokensHandler = () => {
  if (timer) clearTimeout(timer);
};

export const initAuthStore = async () => {
  if (!initialized) {
    initialized = true;
    await refreshTokensIfRequired();
  }
};
