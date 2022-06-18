import {
  StepDefinition,
  StepsDefinition,
  StepsResult
} from '$components/Stepper/types'
import { Components } from '$types/api'

export type AccountInformation = {
  accountType?: ProfileKind;
  tradeName?: string;
  corporateName?: string;
  siret?: string;
  email?: string;
  phone?: string;
}

export type AccountAddresses = {
  legalAddress?: Components.Schemas.NamedAddressDto | undefined;
  billingAddress?: Components.Schemas.NamedAddressDto | undefined;
  shippingAddress?: Components.Schemas.NamedAddressDto | undefined;
  deliveryAddress?: Components.Schemas.NamedAddressDto | undefined;
}

export type AccountConfigurationSteps = StepsDefinition & {
  information: StepDefinition<AccountInformation>;
  addresses: StepDefinition<AccountAddresses>;
}

export type AccountConfigurationResults = StepsResult & {
  information: AccountInformation;
  addresses: AccountAddresses;
}
