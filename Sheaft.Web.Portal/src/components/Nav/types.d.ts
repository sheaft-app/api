export interface IEntry {
  path?: string;
  name: string;
  icon?: string;
  public?: boolean;
  anonymous?: boolean;
  pages?: IEntry[];
  roles?: string[];
  visible: boolean;
  referenced: boolean;
  default: boolean;
  parent?: IEntry;
}
