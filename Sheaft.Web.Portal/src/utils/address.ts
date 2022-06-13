export const formatAddress = value => {
  if(!value) return '';
  const complement = !!value.complement ? value.complement + '<br/>' : '';
  return `${value.street}<br/>${complement}${value.postcode} ${value.city}`;
};
