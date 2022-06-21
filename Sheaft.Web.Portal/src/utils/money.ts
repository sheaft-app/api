export const decimal = (num: number): number => {
  return Math.round((num + Number.EPSILON) * 100) / 100;
};

export const calculateOnSalePrice = (
  wholeSalePrice: number | undefined,
  vat: number | undefined,
  quantity: number = 1
): number => {
  return decimal(quantity * (wholeSalePrice ?? 0) * (1 + (vat ?? 0) / 100));
};

export const calculateWholeSalePrice = (
  wholeSalePrice: number | undefined,
  quantity: number = 1
): number => {
  return decimal(quantity * (wholeSalePrice ?? 0));
};

export const calculateVatPrice = (
  wholeSalePrice: number | undefined,
  vat: number | undefined,
  quantity: number = 1
): number => {
  return decimal(quantity * (wholeSalePrice ?? 0) * ((vat ?? 0) / 100));
};

export const currency = value => {
  let formatter = new Intl.NumberFormat("fr-FR", {
    style: "currency",
    currency: "EUR"
  });

  return formatter.format(value);
};
