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

      const user = get(authStore.user);
      config.headers = {
        Authorization: `${user.tokens.tokenType} ${user.tokens.accessToken}`
      };

      return config;
    },
    function (error) {
      return Promise.reject(error);
    }
  );
};
