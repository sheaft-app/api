import { writable } from 'svelte/store';
import qs from "qs";

const { subscribe:authenticatedSub, set:setIsAuthenticated } = writable(false);

const login = (username: string, password: string): Promise<string> => {
  //TODO call api
  setIsAuthenticated(true);
  let search = window.location.search.replace('?', '');
  return Promise.resolve(search.indexOf('returnUrl') > -1 ? 
    qs.parse(search)["returnUrl"] : '/');
}

export const authStore = { 
  isAuthenticated : {subscribe:authenticatedSub},
  login
};
