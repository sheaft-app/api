export type ReturnableForm = {
  id?: string;
  name: string;
  unitPrice: number;
  vat: number;
  hasVat: boolean;
  code?: string | null | undefined;
};
