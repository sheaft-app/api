import { Request } from "jimmy-js";
import { ProfileKind } from "$components/Account/enums";
import type { AccountAddresses, AccountInformation } from "$components/Account/types";
import type { Client } from "$types/api";
import type { Components } from "$types/api";

export class ConfigureAccountCommand extends Request<Promise<boolean>> {
  constructor(
    public accountType: ProfileKind,
    public information: AccountInformation,
    public addresses: AccountAddresses
  ) {
    super();
  }
}

export class ConfigureAccountHandler {
  constructor(private _client: Client) {}

  handle = async (request: ConfigureAccountCommand): Promise<boolean> => {
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
