import type { ProfileKind, ProfileStatus } from "$enums/profile";

export interface IUser {
  id?: string | null;
  username?: string | null;
  name?: string | null;
  firstname?: string | null;
  lastname?: string | null;
  email?: string | null;
  roles: string [];
  profile: {
    id?: string | null;
    name?: string | null;
    status: ProfileStatus;
    kind?: ProfileKind | null;
  };
}
