import type {
  IAccountAddresses,
  IAccountInformation
} from "$commands/account/configureAccount";
import type {
  IStepResult,
  IStepDefinition,
  IStepsDefinition,
  IStepsResult
} from "$components/Stepper/types/stepper";

export interface IAccountConfigurationSteps extends IStepsDefinition {
  information: IStepDefinition<IAccountInformation>;
  addresses: IStepDefinition<IAccountAddresses>;
}

export interface IAccountConfigurationResults extends IStepsResult {
  information: IAccountInformation;
  addresses: IAccountAddresses;
}
