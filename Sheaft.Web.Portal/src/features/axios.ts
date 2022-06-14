import { OpenAPIClientAxios } from "openapi-client-axios";
import { get } from "svelte/store";
import type { Client } from '$features/api'
import { authStore } from "$components/Auth/auth";

export const api = new OpenAPIClientAxios({
  definition: `${import.meta.env.VITE_API_URL}/swagger/v1/swagger.json`
});

export const configureAxios = (client: Client) => {
  client.interceptors.request.use(
    config => {
      config.baseURL = import.meta.env.VITE_API_URL;
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
