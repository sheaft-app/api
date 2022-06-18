import { create, enforce, test } from "vest";
import type { CreateProductForm, UpdateProductForm } from "$components/Products/types";

export const suite = create(
  "product-suite",
  (data: CreateProductForm | UpdateProductForm) => {
    test("name", "Le nom est requis", () => {
      enforce(data.name).isNotEmpty();
    });

    test("unitPrice", "Le prix est requis", () => {
      enforce(data.unitPrice).isNotEmpty();
    });

    test("unitPrice", "Le prix doit être supérieur à 0€", () => {
      enforce(data.unitPrice).greaterThan(0);
    });

    test("returnableId", "Vous devez selectionner une consigne", () => {
      if (data.hasReturnable) enforce(data.returnableId).isNotNull();
    });

    test("vat", "Le taux de TVA doit être 5.5%, 10% ou 20%", () => {
      enforce(data.vat).inside([5.5, 10, 20]);
    });
  }
);
