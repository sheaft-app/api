import { create, enforce, test } from "vest";
import type { AgreementDeliveryForm } from '$components/Agreements/types'

export const suite = create(
  "agreement-suite",
  (data: AgreementDeliveryForm) => {
    test("limitOrderHourOffset", "L'heure limite de prise de commande est requise", () => {
      if(data.limitEnabled)
        enforce(data.limitOrderHourOffset).isNotNull();
    });
    
    test("limitOrderHourOffset", "L'heure limite doit être supérieure à 0", () => {
      if(data.limitEnabled)
        enforce(data.limitOrderHourOffset).greaterThan(0);
    });
    
    test("deliveryDays", "Vous devez choisir au moins un jour de livraison", () => {
      enforce(data.deliveryDays).lengthNotEquals(0);
    });
  }
);
