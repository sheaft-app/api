export interface IResult {
  success: boolean;
  message?: string;
  error?: any
}

export interface IDataResult<T> extends IResult{
  data: T
}
