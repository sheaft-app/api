import type { GotoHelper } from '@roxi/routify'

export interface IAppModule{
  register():void;
  goToHome():void;
} 

export class AppModule implements IAppModule {
  protected _goto: GotoHelper | null = null
  
  register = (): void => {
  }
  
  goToHome = () => {
    this.navigate('/');
  }

  setGoto = (goto: GotoHelper) => {
    this._goto = goto
  }

  protected navigate = (url:string) : void => {
    if (this._goto)
      this._goto(url)
    else
      throw '$goto was not provided when called getXXXModule($goto)'
  }
}
