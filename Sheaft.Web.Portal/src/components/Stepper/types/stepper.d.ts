export interface IStepDefinition<T> {
  index:number;
  name:string;
  component:any;  
  icon?:string|null;
  initialValues?:T;
}

export interface IStepResult<T> {
  values:T;
}

export interface IStepsDefinition extends Record<string, IStepDefinition>{};
export interface IStepsResult extends Record<string, IStepResult>{};
