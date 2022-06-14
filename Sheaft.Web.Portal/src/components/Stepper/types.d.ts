export interface IStepDefinition<T> {
  id?: string;
  name: string;
  component: any;
  icon?: string | null;
  initialValues?: T;
}

export interface IStepsDefinition extends Record<string, IStepDefinition> {}
export interface IStepsResult extends Record<string, any> {}
