import { getAuthStore } from "$stores/auth";
import apiConfig from "$settings/api";
import axios from "axios";
import { get } from "svelte/store";

export const configureAxios = () => {
  axios.interceptors.request.use(
    function (config) {
      config.baseURL = apiConfig.url;

      const authStore = getAuthStore();
      if (!get(authStore.isAuthenticated)) return config;

      const tokens = get(authStore.tokens);
      config.headers = {
        Authorization: `${tokens.tokenType} ${tokens.accessToken}`
      };

      return config;
    },
    function (error) {
      return Promise.reject(error);
    }
  );
};
