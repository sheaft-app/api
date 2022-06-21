export interface IModalResult<T> {
  isSuccess: boolean;
  value: T;
  error?: any;
}

export class ModalResult<T> implements IModalResult<T> {
  public isSuccess: boolean = false;
  public value: T = null;
  public error?: any = null;

  private constructor(success: boolean, value?: T, error?: any) {
    this.isSuccess = success;
    this.value = value;
    this.error = error;
  }

  static Success = value => {
    return new ModalResult(true, value);
  };
  static Failure = (error?: any) => {
    return new ModalResult(false, null, error);
  };
}
