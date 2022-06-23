export enum OrderStatus{
  Draft,
  Pending,
  Accepted,
  Fulfilled,
  Completed,
  Refused,
  Cancelled
}

export enum DeliveryStatus{
  Pending,
  Scheduled,
  Delivered,
}

export enum LineKind{
  Product, 
  Returnable,
  ReturnedReturnable
}

export enum OrderTab {
  Draft = "Brouillon",
  Sent = "Envoyées",
  Pending = "En attente",
  InProgress = "En cours",
  Delivered = "Livrées",
  Aborted = "Avortées"
}
