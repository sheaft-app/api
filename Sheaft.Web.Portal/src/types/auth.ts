import type { IAddress } from './address'
import type { ProfileKind, ProfileStatus } from '$enums/profile'

export interface IUser {
  id: string;
  username: string;
  name: string,
  firstname: string,
  lastname: string,
  email: string;
  roles: [],
  profile: {
    id: string,
    name: string,
    status: ProfileStatus,
    kind: ProfileKind
  }
}

export interface IRegisterAccount {
  email: string;
  password: string;
  confirm: string,
  firstname: string,
  lastname: string
}

export interface IConfigureAccount {
  tradeName: string;
  corporateName: string;
  siret: string;
  email: string;
  phone: string;
  legalAddress: IAddress;
}

export interface IConfigureSupplier extends IConfigureAccount {
  billingAddress?: IAddress;
  shippingAddress?: IAddress;
}

export interface IConfigureCustomer extends IConfigureAccount {
  billingAddress?: IAddress;
  deliveryAddress?: IAddress;
}
