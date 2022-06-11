import { round } from "$utils/number";

export const calculateOnSalePrice = (
  wholeSalePrice: number | undefined,
  vat: number | undefined
): number => {
  return round((wholeSalePrice ?? 0) * (1 + (vat ?? 0) / 100));
};
