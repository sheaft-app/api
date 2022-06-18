export type CreateReturnableForm = {
  name: string;
  unitPrice: number;
  vat: number;
  hasVat: boolean;
  code?: string | null | undefined;
};

export type UpdateReturnableForm = CreateReturnableForm & {
  id: string;
};
