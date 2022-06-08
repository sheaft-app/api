export interface IModule{
  register():void
} 

export class AppModule implements IModule{
  register = (): void => {
  }
}
