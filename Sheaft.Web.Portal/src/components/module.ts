import type { GotoHelper } from "@roxi/routify";
import type { Request } from "jimmy-js/types/request";
import type { AnyType, Constructor, Handler } from "jimmy-js/types/types";
import type { Notification } from "jimmy-js/types/notification";
import { mediator } from '$components/mediator'

export interface IAppModule {
  registerHandlers(): void;
  goToHome(): void;
}

export abstract class AppModule implements IAppModule {
  protected _goto: GotoHelper | null = null;

  protected constructor() {}

  abstract registerHandlers(): void;

  goToHome = () => {
    this.navigate("/");
  };

  setGoto = (goto: GotoHelper) => {
    this._goto = goto;
  };

  protected registerHandler = <TRequest extends Request<AnyType> | Notification>(
    constructor: Constructor<TRequest>,
    handler: Handler<TRequest>
  ): void => {
    mediator.handle(constructor, handler);
  };

  protected navigate = (url: string): void => {
    if (this._goto) this._goto(url);
    else throw "$goto was not provided when called getXXXModule($goto)";
  };
}
