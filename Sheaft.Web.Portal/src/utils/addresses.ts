import type { IAddress } from '$components/Addresses/types'

export const address = (value:IAddress|undefined) => {
  if(!value) return '';
  const complement = !!value.complement ? value.complement + '<br/>' : '';
  return `${value.street}<br/>${complement}${value.postcode} ${value.city}`;
};
