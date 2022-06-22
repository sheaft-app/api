export type OrderForm = {
};

export type ProductQuantity = {
  productIdentifier:string;
  quantity:number;
}

export type DraftLine = {
  id: string; 
  name: string; 
  unitPrice: number; 
  vat: number; 
  quantity:number;
  returnable?:any; 
}

export type DeliveryLine = ProductQuantity & { 
  batchIdentifiers?: string[];
}
