import { create, enforce, test } from "vest";
import type {  ReturnableForm} from "$components/Returnables/types";

export const suite = create(
  "returnable-suite",
  (data: ReturnableForm) => {
    test("name", "Le nom est requis", () => {
      enforce(data.name).isNotEmpty();
    });

    test("unitPrice", "Le prix est requis", () => {
      enforce(data.unitPrice).isNotEmpty();
    });

    test("unitPrice", "Le prix doit être supérieur à 0€", () => {
      enforce(data.unitPrice).greaterThan(0);
    });

    test("vat", "Vous devez selectionner un taux de TVA", () => {
      if (data.hasVat) enforce(data.vat).isNotNull();
    });

    test("vat", "Le taux de TVA doit être 5.5%, 10% ou 20%", () => {
      if (data.hasVat) enforce(data.vat).inside([5.5, 10, 20]);
      else enforce(data.vat).equals(0);
    });
  }
);
