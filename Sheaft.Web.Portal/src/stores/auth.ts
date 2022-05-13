import { writable } from "svelte-local-storage-store";
import { derived, get } from "svelte/store";
import axios from "axios";
import jwt_decode from "jwt-decode";

let timer = null;
let initialized = false;

const userStore = writable("auth", {
  tokens: { accessToken: "", refreshToken: "", expiresAt: null, tokenType: "" },
  isAuthenticated: false,
  profile: {
    id: "",
    username: "",
    email: "",
    roles: []
  }
});

const login = async (username: string, password: string): Promise<boolean> => {
  try {
    const result = await axios.post("/api/token/login", { username, password });

    userStore.update(as => {
      as.isAuthenticated = true;
      as.tokens = getTokens(result.data);
      as.profile = getUserFromAccessToken(as.tokens.accessToken);
      return as;
    });

    configureRefreshTokensHandler();

    return Promise.resolve(true);
  } catch (e) {
    return Promise.reject(false);
  }
};

const logout = (): boolean => {
  try {
    userStore.update(as => {
      as.isAuthenticated = false;
      as.tokens = getTokens();
      as.profile = getUserFromAccessToken();
      return as;
    });

    clearRefreshTokensHandler();

    return true;
  } catch (e) {
    return false;
  }
};

const refreshToken = async () => {
  const user = get(userStore);
  try {
    const result = await axios.post("/api/token/refresh", {
      token: user.tokens.refreshToken
    });

    userStore.update(as => {
      as.tokens = getTokens(result.data);
      as.profile = getUserFromAccessToken(as.tokens.accessToken);
      return as;
    });
  } catch (e) {
    console.error("An error occured while refreshing access token");
  }
}

const isInRoles = (roles?: Array<string>): boolean => {
  if (!roles) return true;

  const user = get(userStore);
  if (!user.profile.roles) user.profile.roles = [];

  const userInRoles = roles.filter(r => user.profile.roles.includes(r));
  return userInRoles.length > 0;
};

const getUserFromAccessToken = (access_token?) => {
  const user = { id: "", username: "", email: "", roles: [] };
  if (!access_token) return user;

  const decoded = jwt_decode<any>(access_token);

  user.id = decoded.sub;
  user.username =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
  user.email =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
  user.roles = JSON.parse(
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/role"] ?? "[]"
  );

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
  tokens.tokenType = data.tokenType;

  return tokens;
};

const refreshTokensIfRequired = async () => {
  let user = get(userStore);
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

const authStore = {
  user: derived([userStore], ([$userStore]) => $userStore.profile),
  isAuthenticated: derived([userStore], ([$userStore]) => $userStore.isAuthenticated),
  isInRoles,
  login,
  logout,
  refreshToken
};

export const getAuthStore = () => {
  if (!initialized) {
    initialized = true;
    refreshTokensIfRequired();
  }

  return authStore;
};
