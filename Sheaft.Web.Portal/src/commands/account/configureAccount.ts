import type { Client, Components } from "$types/api";
import { Request } from "jimmy-js";
import { ProfileKind } from "$enums/profile";

export interface IAccountInformation {
  accountType?: ProfileKind;
  tradeName?: string;
  corporateName?: string;
  siret?: string;
  email?: string;
  phone?: string;
}

export interface IAccountAddresses {
  legalAddress?: Components.Schemas.NamedAddressDto | undefined;
  billingAddress?: Components.Schemas.NamedAddressDto | undefined;
  shippingAddress?: Components.Schemas.NamedAddressDto | undefined;
  deliveryAddress?: Components.Schemas.NamedAddressDto | undefined;
}

export class ConfigureAccountRequest extends Request<Promise<boolean>> {
  constructor(
    public accountType: ProfileKind | undefined,
    public information: IAccountInformation,
    public addresses: IAccountAddresses
  ) {
    super();
  }
}

export class ConfigureAccountRequestHandler {
  constructor(private _client: Client) {}

  handle = async (request: ConfigureAccountRequest): Promise<boolean> => {
    try {
      let body:
        | Components.Schemas.CustomerInfoRequest
        | Components.Schemas.SupplierInfoRequest = {
        tradeName: request.information.tradeName,
        email: request.information.email,
        phone: request.information.phone,
        siret: request.information.siret,
        corporateName: request.information.corporateName,
        legalAddress: request.addresses.legalAddress,
        billingAddress: request.addresses.billingAddress
      };

      switch (request.accountType) {
        case ProfileKind.Customer:
          (<Components.Schemas.CustomerInfoRequest>body).deliveryAddress =
            request.addresses.deliveryAddress;
          await this._client.ConfigureAccountAsCustomer(null, body);
          break;
        case ProfileKind.Supplier:
          (<Components.Schemas.SupplierInfoRequest>body).shippingAddress =
            request.addresses.shippingAddress;
          await this._client.ConfigureAccountAsSupplier(null, body);
          break;
      }
      return Promise.resolve(true);
    } catch (exc) {
      console.error(exc);
      return Promise.reject({ error: exc });
    }
  };
}
