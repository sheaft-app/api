import type { StatusCode } from "$enums/http";

export interface IResult<T> {
  success: boolean;
  status: StatusCode;
  data?: T;
  error?:any;
}

export interface IListResult<T> extends IResult<T> {}
