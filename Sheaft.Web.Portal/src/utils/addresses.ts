import type { IAddress } from '$components/Addresses/types'
import type { Components } from '$features/api'

export const address = (value:IAddress|Components.Schemas.AddressDto|undefined) => {
  if(!value) return '';
  const complement = !!value.complement ? value.complement + '<br/>' : '';
  return `${value.street}<br/>${complement}${value.postcode} ${value.city}`;
};
