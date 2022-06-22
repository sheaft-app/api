export enum OrderStatus{
  Draft,
  Pending,
  Accepted,
  Fulfilled,
  Completed,
  Refused,
  Cancelled
}

export enum OrderTab {
  Draft = "Brouillon",
  Pending = "En attente",
  Active = "Acceptées",
  Completed = "Terminées",
  Aborted = "Avortées"
}
