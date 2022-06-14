export interface IPageAction {
  name: string;
  disabled?:boolean;
  action: string | Function;
  actions?: IPageAction[] | null;
}
