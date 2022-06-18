import { ProfileKind, ProfileStatus } from '$components/Account/enums'

export type Account = {
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

export type Tokens = {
  accessToken?: string;
  refreshToken?: string;
  expiresAt?: Date;
  tokenType?: string;
}

export type AuthState = {
  tokens?: Tokens;
  isAuthenticated: boolean;
  isRegistered: boolean;
  account?: Account;
}
