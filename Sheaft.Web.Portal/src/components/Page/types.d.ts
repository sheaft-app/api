export type PageAction = {
  name: string;
  disabled?: boolean;
  visible?: boolean;
  color?:string;
  action: string | Function;
  actions?: PageAction[] | null;
};
