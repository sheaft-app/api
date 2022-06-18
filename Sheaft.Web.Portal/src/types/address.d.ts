export type Address = {
  street: string | null;
  complement?: string | null;
  postcode: string | null;
  city: string | null;
};

export type NamedAddress = Address & {
  name: string | null;
  email: string | null;
};
