import type { IModalResult } from '$components/Modal/types'

export class ModalResult<T> implements IModalResult<T>{
  public isSuccess:boolean = false;
  public value:T = null;
  
  private constructor(success:boolean, value?:T, error?:any) {
    this.isSuccess = true;
    this.value = value;
  }
  
  static Success = (value) => {
    return new ModalResult(true, value)
  }
  static Failure = (error:any) => {
    return new ModalResult(false, null, error)
  }
}
