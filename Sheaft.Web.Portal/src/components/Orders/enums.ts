export enum OrderStatus{
  Draft,
  Pending,
  Accepted,
  Fulfilled,
  Completed,
  Refused,
  Cancelled
}

export enum OrderLineKind{
  Product, 
  Returnable
}

export enum OrderTab {
  Draft = "Brouillon",
  Sent = "Envoyées",
  Pending = "En attente",
  InProgress = "En cours",
  Delivered = "Livrées",
  Aborted = "Avortées"
}
