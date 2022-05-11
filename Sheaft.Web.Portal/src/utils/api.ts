import { authStore } from '$stores/auth';
import apiConfig from '$configs/api';
import axios from 'axios';
import { get } from 'svelte/store';

export const configureAxios = () => {
  axios.interceptors.request.use(
    function (config) {
      config.baseURL = apiConfig.url;

      if (!get(authStore.isAuthenticated)) 
        return config;

      const user = get(authStore.user);
      config.headers = {
        Authorization: `${user.tokenType} ${user.tokens.accessToken}`
      };

      return config;
    },
    function (error) {
      return Promise.reject(error);
    }
  );
};
