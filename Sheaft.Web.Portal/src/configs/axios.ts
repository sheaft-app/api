import { isAuthenticated, tokens } from "$stores/auth";
import apiConfig from "$settings/api";
import axios from "axios";
import { get } from "svelte/store";

export const configureAxios = () => {
  axios.interceptors.request.use(
    function (config) {
      config.baseURL = apiConfig.url;

      if (!get(isAuthenticated)) return config;

      const access_tokens = get(tokens);
      config.headers = {
        Authorization: `${access_tokens.tokenType} ${access_tokens.accessToken}`
      };

      return config;
    },
    function (error) {
      return Promise.reject(error);
    }
  );
};
