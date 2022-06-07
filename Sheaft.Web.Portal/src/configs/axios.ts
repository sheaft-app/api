import { isAuthenticated, tokens } from "$stores/auth";
import apiConfig from "$settings/api";
import { get } from "svelte/store";
import type { Client } from '$types/api'
import { OpenAPIClientAxios } from 'openapi-client-axios'
import { StatusCode } from '$enums/http'

export const api = new OpenAPIClientAxios({ definition: `${apiConfig.url}/swagger/v1/swagger.json`});

export const configureAxios = (client: Client) => {
  client.interceptors.request.use(
    config => {
      config.baseURL = apiConfig.url;
      config.headers = {
        Timezone: Intl.DateTimeFormat().resolvedOptions().timeZone
      };

      if (!get(isAuthenticated)) return config;

      const {tokenType, accessToken} = get(tokens);
      config.headers.Authorization = `${tokenType} ${accessToken}`;

      return config;
    },
    err => {
      return Promise.reject(err);
    }
  );
};

