import type { StatusCode } from "$enums/http";

export interface IEmptyResult {
  success: boolean;
  status: StatusCode;
  error?: any;
}

export interface IResult<T> extends IEmptyResult {
  data?: T;
}

export interface IListResult<T> extends IResult<T> {}
