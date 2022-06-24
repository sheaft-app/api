import { create, enforce, test } from 'vest'
import type { BatchForm } from '$components/Batches/types'
import { BatchDateKind } from '$components/Batches/enums'

export const suite = create(
  "batch-suite",
  (data: BatchForm) => {
    test("number", "Le numéro de lot est requis", () => {
      enforce(data.number).isNotEmpty();
    });
    test("dateKind", "Le type de date est requis", () => {
      enforce(data.kind).isNotNull();
    });
    test("expirationDate", "La date est requise", () => {
      enforce(data.expirationDate).isNotEmpty();
    });
    test("productionDate", "La date est requise", () => {
      if(data.kind !== BatchDateKind.DDC)
        enforce(data.productionDate).isNotEmpty();
    });
  }
);
