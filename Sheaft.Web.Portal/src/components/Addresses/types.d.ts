export interface IAddress {
  street: string | null;
  complement?: string | null;
  postcode: string | null;
  city: string | null;
}

export interface INamedAddress extends IAddress {
  name: string | null;
  email: string | null;
}
