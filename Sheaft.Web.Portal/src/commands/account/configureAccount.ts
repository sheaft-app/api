import type { Client, Components } from '$types/api'
import { Request } from 'jimmy-js'
import { ProfileKind } from '$enums/profile'

export interface IAccountInformation {
  accountType?: ProfileKind;
  tradeName?: string;
  corporateName?: string;
  siret?: string;
  email?: string;
  phone?: string;
}

export interface IAccountAddresses {
  legalAddress?: Components.Schemas.NamedAddressDto;
  billingAddress?: Components.Schemas.NamedAddressDto;
  shippingAddress?: Components.Schemas.NamedAddressDto;
  deliveryAddress?: Components.Schemas.NamedAddressDto;
}

export class ConfigureAccountRequest extends Request<Promise<boolean>> {
  constructor(
    public accountType: ProfileKind | undefined, 
    public information: IAccountInformation, 
    public addresses: IAccountAddresses) {
    super()
  }
}

export class ConfigureAccountRequestHandler {
  constructor(private _client: Client) {
  }

  handle = async (request: ConfigureAccountRequest): Promise<boolean> => {
    try {
      let apiCalled = false
      switch (request.accountType) {
        case ProfileKind.Customer:
          await this._client.ConfigureAccountAsCustomer(null, {
            tradeName: request.information.tradeName,
            email: request.information.email,
            phone: request.information.phone,
            siret: request.information.siret,
            corporateName: request.information.corporateName,
            legalAddress: request.addresses.legalAddress,
            billingAddress: request.addresses.billingAddress,
            deliveryAddress: request.addresses.deliveryAddress
          })
          apiCalled = true
          break
        case ProfileKind.Supplier:
          await this._client.ConfigureAccountAsSupplier(null, {
            tradeName: request.information.tradeName,
            email: request.information.email,
            phone: request.information.phone,
            siret: request.information.siret,
            corporateName: request.information.corporateName,
            legalAddress: request.addresses.legalAddress,
            billingAddress: request.addresses.billingAddress,
            shippingAddress: request.addresses.shippingAddress,
          })
          apiCalled = true
          break
      }
      return Promise.resolve(apiCalled)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
