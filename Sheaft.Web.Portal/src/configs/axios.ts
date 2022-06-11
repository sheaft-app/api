import { authStore } from "$stores/auth";
import apiConfig from "$settings/api";
import type { Client } from "$types/api";
import { OpenAPIClientAxios } from "openapi-client-axios";
import { get } from "svelte/store";

export const api = new OpenAPIClientAxios({
  definition: `${apiConfig.url}/swagger/v1/swagger.json`
});

export const configureAxios = (client: Client) => {
  client.interceptors.request.use(
    config => {
      config.baseURL = apiConfig.url;
      config.headers = {
        Timezone: Intl.DateTimeFormat().resolvedOptions().timeZone
      };

      const store = get(authStore);
      if (!store.isAuthenticated || !store.tokens) return config;

      const { tokenType, accessToken } = store.tokens;
      config.headers.Authorization = `${tokenType} ${accessToken}`;

      return config;
    },
    err => {
      return Promise.reject(err);
    }
  );
};
