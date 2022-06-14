import type { Components } from '$features/api'

export const statusStr = (status:Components.Schemas.AgreementStatus|string|undefined) => {
  switch (status?.toString()){
    case "0":
      return 'En attente';
    case "1":
      return 'Actif';
    case "2":
      return 'Refusé';
    case "3":
      return 'Annulé';
    default:
      return "Inconnu";
  }
}
