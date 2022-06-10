import type { Client, Components } from '$types/api'
import { Request } from 'jimmy-js'
import { ProfileKind } from '$enums/profile'

export interface IConfigureState {
  info: IConfigureInformation;
  legals: IConfigureLegals;
  localisation: IConfigureLocalisation;
}

export interface IConfigureInformation{
  accountType?:ProfileKind;
  tradeName?:string;
  email?:string;
  phone?:string;
}

export interface IConfigureLegals{
  corporateName?:string;
  siret?:string;
  address?:Components.Schemas.AddressDto;
}

export interface IConfigureLocalisation{
  billingAddress?:Components.Schemas.NamedAddressDto;
  shippingAddress?:Components.Schemas.NamedAddressDto;
  deliveryAddress?:Components.Schemas.NamedAddressDto;
}

export class ConfigureAccountRequest extends Request<Promise<boolean>> {
  constructor(public accountType:ProfileKind | undefined, public state:IConfigureState) {
    super()
  }
}

export class ConfigureAccountRequestHandler {
  constructor(private _client: Client) {
  }

  handle = async (request: ConfigureAccountRequest): Promise<boolean> => {
    try {
      let apiCalled = false;      
      switch (request.accountType) {
        case ProfileKind.Customer:       
          await this._client.ConfigureAccountAsCustomer(null, {
            
          })
          apiCalled = true
          break
        case ProfileKind.Supplier:
          await this._client.ConfigureAccountAsSupplier(null, { 
            
          })
          apiCalled = true
          break;
      }
      return Promise.resolve(apiCalled)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }
}
