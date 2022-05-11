import { writable } from 'svelte-local-storage-store';
import { derived } from 'svelte/store';
import qs from 'qs';
import axios from 'axios';

const userStore = writable('user', {
  tokens: { accessToken: '', refreshToken: '', expiresAt: 3600 },
  tokenType: '',
  isAuthenticated: false,
  username: ''
});

const login = async (username: string, password: string): Promise<string> => {
  try {
    const result = await axios.post('/api/token/login', { username, password });

    userStore.update(as => {
      as.isAuthenticated = true;
      as.tokens.accessToken = result.data.access_token;
      as.tokens.refreshToken = result.data.refresh_token;
      as.tokens.expiresAt = result.data.expires_in;
      as.tokenType = result.data.token_type;
      as.username = username;
      return as;
    });

    let search = window.location.search.replace('?', '');
    return Promise.resolve(search.indexOf('returnUrl') > -1 ? qs.parse(search)['returnUrl'] : '/');
  }
  catch(e){
    console.log(e);
    return Promise.reject(e);
  }
};

export const authStore = {
  user: derived([userStore], ([$userStore]) => $userStore),
  isAuthenticated: derived([userStore], ([$userStore]) => $userStore.isAuthenticated),
  login
};
