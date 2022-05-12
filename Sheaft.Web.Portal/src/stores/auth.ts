import { writable } from "svelte-local-storage-store";
import { derived, get } from "svelte/store";
import qs from "qs";
import axios from "axios";

let timer = null;
let initialized = false;

const userStore = writable("user", {
  tokens: { accessToken: "", refreshToken: "", expiresAt: null, tokenType: "" },
  isAuthenticated: false,
  username: ""
});

const login = async (username: string, password: string): Promise<string> => {
  try {
    const result = await axios.post("/api/token/login", { username, password });

    userStore.update(as => {
      as.isAuthenticated = true;
      as.tokens = getTokens(result.data);
      as.username = username;
      return as;
    });

    configureRefreshTokensHandler();

    let search = window.location.search.replace("?", "");
    return Promise.resolve(
      search.indexOf("returnUrl") > -1 ? qs.parse(search)["returnUrl"] : "/"
    );
  } catch (e) {
    return Promise.reject(e);
  }
};

const logout = (): boolean => {
  try {
    userStore.update(as => {
      as.isAuthenticated = false;
      as.tokens = getTokens();
      as.username = "";
      return as;
    });

    clearRefreshTokensHandler();

    return true;
  } catch (e) {
    return false;
  }
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
  if (minRefreshDate <= new Date()) 
    return;

  try {
    const result = await axios.post("/api/token/refresh", {
      token: user.tokens.refreshToken
    });

    userStore.update(as => {
      as.tokens = getTokens(result.data);
      return as;
    });
  } catch (e) {
    console.error("An error occured while refreshing access token");
  }

  configureRefreshTokensHandler();
};

const configureRefreshTokensHandler = () => {
  timer = setTimeout(refreshTokensIfRequired, 60000);
};

const clearRefreshTokensHandler = () => {
  if (timer) clearTimeout(timer);
};

const authStore = {
  user: derived([userStore], ([$userStore]) => $userStore),
  isAuthenticated: derived([userStore], ([$userStore]) => $userStore.isAuthenticated),
  login,
  logout
};

export const getAuthStore = () => {
  if (!initialized) {
    initialized = true;
    refreshTokensIfRequired();
  }

  return authStore;
};
