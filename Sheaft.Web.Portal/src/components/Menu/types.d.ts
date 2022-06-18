export type MenuElement = {
  path?: string;
  name: string;
  icon?: string;
  public?: boolean;
  anonymous?: boolean;
  roles?: string[];
  visible: boolean;
  referenced: boolean;
};

export type MenuGroup = MenuElement & {
  pages?: MenuEntry[];
};

export type MenuEntry = MenuElement & {
  path: string;
  default: boolean;
  parent?: MenuGroup;
};
