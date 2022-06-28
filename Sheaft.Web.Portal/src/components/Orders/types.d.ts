export type OrderForm = {
};

export type ProductQuantity = {
  productIdentifier:string;
  quantity:number;
}

export type LineQuantity = {
  identifier:string;
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

export type BatchSelection = { label: string, value: string };

export type DeliveryLine = ProductQuantity & { 
  batchIdentifiers?: string[];
}

export type DeliveryLineCompletion = ProductQuantity & { 
  batchIdentifiers?: BatchSelection[];
}
