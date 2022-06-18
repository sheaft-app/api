import type { Address } from "$types/address";
import type { Components } from "$types/api";

export const address = (value: Address | Components.Schemas.AddressDto | undefined) => {
  if (!value) return "";
  const complement = !!value.complement ? value.complement + "<br/>" : "";
  return `${value.street}<br/>${complement}${value.postcode} ${value.city}`;
};
