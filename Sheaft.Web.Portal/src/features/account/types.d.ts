import { ProfileKind } from '$enums/profile'
import { IStepDefinition, IStepsDefinition, IStepsResult } from '$components/Stepper/types'
import type { Components } from '$features/api'

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

export interface IAccountConfigurationSteps extends IStepsDefinition {
  information: IStepDefinition<IAccountInformation>;
  addresses: IStepDefinition<IAccountAddresses>;
}

export interface IAccountConfigurationResults extends IStepsResult {
  information: IAccountInformation;
  addresses: IAccountAddresses;
}
