export interface IAddress {
  street: string;
  complement?: string;
  postcode: string;
  city: string;
}

export interface INamedAddress extends IAddress {
  name: string;
  email: string;
}
