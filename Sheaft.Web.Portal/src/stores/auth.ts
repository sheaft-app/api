import { derived, writable } from 'svelte/store';
import type { Writable, Readable } from 'svelte/store';

class AuthStore {
  constructor(private _authenticated: Writable<boolean> = writable(false)) {}

  get isAuthenticated(): Readable<boolean> {
    return derived([this._authenticated], ([$_authenticated]) => $_authenticated);
  }

  login(username: string, password: string): Promise<boolean> {
    this._authenticated.set(true);
    return Promise.resolve(true);
  }
}

// Export a singleton
export const authStore = new AuthStore();
