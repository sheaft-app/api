import type { BatchDateKind } from '$components/Batches/enums'

export const dateKind = (value: BatchDateKind | undefined) => {
  switch (value?.toString()) {
    case "0":
      return 'Date limite de consommation'
    case "1":
      return 'Date de durabilité minimale'
    case "2":
      return 'Date de congélation'
    case "3":
      return 'Date de consommation recommandée'
    default:
      return 'Inconnu';
  }
}

export const dateKindShort = (value: BatchDateKind | undefined) => {
  switch (value?.toString()) {
    case "0":
      return 'DLC'
    case "1":
      return 'DDM'
    case "2":
      return 'DDC'
    case "3":
      return 'DCR'
    default:
      return 'Inconnu';
  }
}
