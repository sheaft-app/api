export interface IModalResult<T> {
  isSuccess: boolean;
  value: T
  error?: any
}

export class ModalResult<T> implements IModalResult<T>{
  private constructor(value:T) {
    this.isSuccess = true;
    this.value = value;
  }
  private constructor(success: boolean, error:any) {
    this.isSuccess = success;
    this.error = error;
  }
  
  static Success = (value: T) => {
    return new ModalResult(value)
  }
  static Failure = (error:any) => {
    return new ModalResult(false, error)
  }
}
