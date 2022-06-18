import type { MenuElement, MenuEntry, MenuGroup } from "./types";

const parseAndAssignToMenus = (
  children: ClientNodeApi[],
  menus: Record<string, MenuGroup | MenuEntry>,
  parent?: string,
  roles?: string[]
) => {
  children.forEach((c: ClientNodeApi) => {
    let menuName = parent ?? c.meta.menu;
    if (c.children?.length > 0) {
      if (c.__file.isDir && c.__file.isFile && !menus[menuName]) {
        menus[menuName] = getMenuElement(c, null, null, c.meta.roles, true);
      }
    } else if (menus[menuName]) {
      (<MenuGroup>menus[menuName]).pages.push(
        <MenuEntry>getMenuElement(c, null, menus[menuName], menus[menuName].roles)
      );
    } else {
      menus[c.path] = <MenuEntry>getMenuElement(c);
    }

    if (c.children && c.children.length > 0)
      parseAndAssignToMenus(c.children, menus, menuName, c.meta.roles ?? roles);
  });

  return menus;
};

const getMenuElement = (
  route: ClientNodeApi,
  title?: string | null,
  parent?: MenuGroup,
  roles?: string[],
  isGroup: boolean = false
): MenuElement => {
  if (!isGroup)
    return <MenuEntry>{
      path: route.path,
      name: title ?? route.meta.menu,
      icon: route.meta.icon,
      public: route.meta.public,
      anonymous: route.meta.anonymous,
      visible: route.meta.menu?.length > 0,
      referenced: (route.meta.index > 0 || route.meta.index) ?? false,
      default: route.meta.default ?? false,
      roles: parseRoles(route.meta.roles ?? roles) ?? [],
      parent: parent
    };

  return <MenuGroup>{
    path: route.path,
    name: title ?? route.meta.menu,
    icon: route.meta.icon,
    public: route.meta.public,
    anonymous: route.meta.anonymous,
    visible: route.meta.menu?.length > 0,
    referenced: (route.meta.index > 0 || route.meta.index) ?? false,
    roles: parseRoles(route.meta.roles ?? roles) ?? [],
    pages: []
  };
};

export const parseLayoutTree = (children: ClientNodeApi[]) => {
  return parseAndAssignToMenus(children, {});
};

export const canHighlightMenuItem = (entry: MenuEntry, currentPath: string): boolean => {
  if (currentPath == entry.path) return true;

  if (entry.parent)
    //should highlight current menu item => /sub only if /sub/1 is found but not visible in menu
    return (
      entry.parent.pages.find((p: MenuEntry) => !p.visible && p.path == currentPath) !=
      null
    );

  return false;
};

export const entryIsGroup = (entry: MenuElement): boolean => {
  let group = entry as MenuGroup;
  return group.pages?.length > 0;
};

export const entryIsPage = (entry: MenuElement): boolean => {
  let page = entry as MenuEntry;
  return page.path?.length > 0;
};

export const canDisplayMenuElement = (
  entry: MenuElement,
  isAuthenticated: boolean,
  isInRoles: Function
): boolean => {
  if (!entry.visible) return false;
  if (entry.public) return true;
  if (!isAuthenticated) return false;
  if (entry.roles?.length > 0) return isInRoles(entry.roles);
  return true;
};

const parseRoles = (roles: string | string[]): string[] => {
  if (typeof roles == "string" && (<string>roles).indexOf("[") > -1)
    return JSON.parse((<string>roles).replace(/'/g, '"'));

  return <string[]>roles;
};

export const parseActivePath = (path: string): string => {
  if (path?.length < 1) return "/index";

  let resultPath = path;
  let indexOf = resultPath.indexOf("/");
  if (indexOf > -1 && resultPath.indexOf("/") == resultPath.length - 1)
    resultPath += "index";

  let split = resultPath.split("/:");
  if (split && split.length > 1) resultPath = split[0] + "/index";

  return resultPath;
};

export const parseSubActivePath = (path: string): string => {
  if (path?.length < 1) return "/index";

  let resultPath = path;
  let indexOf = resultPath.indexOf("/");
  if (indexOf > -1 && resultPath.indexOf("/") == resultPath.length - 1)
    resultPath += "index";

  return resultPath;
};
