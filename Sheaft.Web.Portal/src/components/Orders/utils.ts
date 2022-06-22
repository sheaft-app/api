import type { Components } from "$types/api";

export const orderStatus = (order: Components.Schemas.OrderDto | Components.Schemas.OrderDetailsDto) => {
  switch (order?.status?.toString()) {
    case "0":
      return "Brouillon";
    case "1":
      return "En attente";
    case "2":
      return "En cours";
    case "3":
      return "Prête";
    case "4":
      return "Livrée";
    case "5":
      return "Refusée";
    case "6":
      return "Annulée";
    default:
      return "Inconnu";
  }
};
