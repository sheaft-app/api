export type ReturnableOption = {
  label: string;
  value: string;
};

export type CreateProductForm = {
  name: string;
  unitPrice: number;
  vat: number;
  hasReturnable: boolean;
  code?: string | null | undefined;
  description?: string | null | undefined;
  returnableId?: string | null | undefined;
};

export type UpdateProductForm = CreateProductForm & {
  id: string;
};
