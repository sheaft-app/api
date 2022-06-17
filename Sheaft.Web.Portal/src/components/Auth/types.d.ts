import { ProfileKind, ProfileStatus } from '$components/Auth/profile'
import type { Components } from '$features/api'
import type { Readable } from 'svelte/store'

export interface IAccount {
  id?: string | null;
  username?: string | null;
  name?: string | null;
  firstname?: string | null;
  lastname?: string | null;
  email?: string | null;
  roles: string[];
  profile: {
    id?: string | null;
    name?: string | null;
    status: ProfileStatus;
    kind?: ProfileKind | null;
  };
}

export interface ITokens {
  accessToken?: string;
  refreshToken?: string;
  expiresAt?: Date;
  tokenType?: string;
}

export interface IAuthState {
  tokens?: ITokens;
  isAuthenticated: boolean;
  isRegistered: boolean;
  account?: IAccount;
}

export interface IAuthStore extends Readable<IAuthState> {
  userIsInRoles(roles?: string[]): boolean;
  setConnectedUser(token: Components.Schemas.TokenResponse): void;
  clearConnectedUser(): void;
  startMonitorUserAccessToken(): Promise<void>;
}
