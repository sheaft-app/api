export type StepDefinition<T> = {
  id?: string;
  name: string;
  component: any;
  icon?: string | null;
  initialValues?: T;
}

export type StepsDefinition = Record<string, IStepDefinition>;
export type StepsResult = Record<string, any>;
