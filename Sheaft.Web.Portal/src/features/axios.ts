import { OpenAPIClientAxios } from 'openapi-client-axios'
import { get } from "svelte/store";
import type { Client } from '$features/api'
import type {Document} from 'openapi-client-axios'
import { authStore } from "$components/Auth/auth";
import swagger from "$features/openapi/swagger.json"

export const api = new OpenAPIClientAxios({
  definition: <Document>swagger
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
