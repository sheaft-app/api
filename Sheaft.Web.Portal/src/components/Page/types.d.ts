export type PageAction = {
  name: string;
  disabled?:boolean;
  action: string | Function;
  actions?: PageAction[] | null;
}
