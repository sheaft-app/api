export enum AgreementStatus {
  Pending,
  Active,
  Refused,
  Cancelled
}

export enum AgreementOwner {
  Supplier,
  Customer
}

export enum AgreementTab {
  Active = 'Actifs',
  Sent = 'Demandes envoyées',
  Received = 'Demandes reçues',
  Other = 'Autres'
}
