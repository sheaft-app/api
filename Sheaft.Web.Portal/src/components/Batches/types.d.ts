import { BatchDateKind } from '$components/Batches/enums'

export type BatchForm = {
  id?:string;
  number: string;
  kind: BatchDateKind;
  expirationDate: string;
  productionDate: string;
};
