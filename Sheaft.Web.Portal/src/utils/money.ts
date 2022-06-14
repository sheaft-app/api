export const decimal = (num: number): number => {
  return Math.round((num + Number.EPSILON) * 100) / 100;
};

export const calculateOnSalePrice = (
  wholeSalePrice: number | undefined,
  vat: number | undefined
): number => {
  return decimal((wholeSalePrice ?? 0) * (1 + (vat ?? 0) / 100));
};

export const currency = value => {
  let formatter = new Intl.NumberFormat("fr-FR", {
    style: "currency",
    currency: "EUR"
  });

  return formatter.format(value);
};
