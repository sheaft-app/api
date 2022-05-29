import { isAuthenticated, tokens } from "$stores/auth";
import apiConfig from "$settings/api";
import { get } from "svelte/store";
import type { Client } from '$types/api'
import { OpenAPIClientAxios } from 'openapi-client-axios'

export const api = new OpenAPIClientAxios({ definition: `${apiConfig.url}/swagger/v1/swagger.json`});

export const configureAxios = (client: Client) => {
  client.interceptors.request.use(
    function (config) {
      config.baseURL = apiConfig.url;
      config.headers = {
        Timezone: Intl.DateTimeFormat().resolvedOptions().timeZone
      };

      if (!get(isAuthenticated)) return config;

      const access_tokens = get(tokens);
      config.headers.Authorization = `${access_tokens.tokenType} ${access_tokens.accessToken}`;

      return config;
    },
    function (error) {
      return Promise.reject(error);
    }
  );
};
