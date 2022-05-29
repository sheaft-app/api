export const format = (node, formatFunction) => {
  node.innerHTML = formatFunction(node.innerHTML)
  return {  }
}

export const currency = value => {
  let formatter = new Intl.NumberFormat('fr-FR', {
    style: 'currency',
    currency: 'EUR',
  });
  
  return formatter.format(value);
}

export const percent = value => {
  let formatter = new Intl.NumberFormat('fr-FR', {
    style: 'percent',
    minimumSignificantDigits: 2
  });
  
  if(value <= 1)
    return formatter.format(value);

  return formatter.format(value / 100);
}
