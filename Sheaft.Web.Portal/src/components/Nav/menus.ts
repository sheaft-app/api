import type { IEntry } from '$components/Nav/types'

const parseAndAssignToMenus = (children: any, menus: any, parent?: string, roles?: string[]) => {
  children.forEach((c: any) => {
    let selector = c.meta.group ? c.meta.menu : parent;

    //insert or update group
    if (selector) {
      if (c.__file.isDir && c.__file.isFile) {
        if (!menus[selector]) menus[selector] = getMenuEntry(c, c.meta.group, null, c.meta.roles);

        menus[selector].public = c.meta.public;
        menus[selector].icon = c.meta.icon;
        menus[selector].name = c.meta.menu;
        menus[selector].roles = parseRoles(c.meta.roles ?? roles) ?? [];
        menus[selector].visible = c.meta.menu?.length > 0;
        menus[selector].referenced = (c.meta.index > 0 || c.meta.index) ?? false;
      } else {
        menus[selector].pages.push(getMenuEntry(c, null, menus[selector], menus[selector].roles));
      }
    } else {
      menus[c.path] = getMenuEntry(c);
    }

    if (c.children && c.children.length > 0)
      parseAndAssignToMenus(c.children, menus, selector, c.meta.roles ?? roles);
  });

  return menus;
};

const getMenuEntry = (route: any, title?: string | null, parent?: any, roles?: string[]) => {
  return {
    path: route.path,
    name: title ?? route.meta.menu,
    icon: route.meta.icon,
    public: route.meta.public,
    anonymous: route.meta.anonymous,
    pages: [],
    visible: route.meta.menu?.length > 0,
    referenced: (route.meta.index > 0 || route.meta.index) ?? false,
    default: route.meta.default ?? false,
    roles: parseRoles(route.meta.roles ?? roles) ?? [],
    parent: parent
  };
};

export const parseLayoutTree = (children: any) => {
  return parseAndAssignToMenus(children, {});
};

export const canHighlightMenuItem = (entry: any, currentPath: string): boolean => {
  if (currentPath == entry.path) return true;

  if (entry.parent)
    //should highlight current menu item => /sub only if /sub/1 is found but not visible in menu
    return (
      entry.parent.pages.find((p: IEntry) => !p.visible && p.path == currentPath) != null
    );

  return false;
};

export const entryIsGroup = (entry: IEntry): boolean => {
  return entry.pages != null && entry.pages.length > 0;
};

export const entryIsPage = (entry: IEntry): boolean => {
  return entry.path != null && entry.path.length > 0;
};

export const canDisplayEntry = (
  entry: IEntry,
  isAuthenticated: boolean,
  isInRoles: Function
): boolean => {
  if (!entry.visible) return false;

  if (entry.public) return true;

  if (!isAuthenticated) return false;

  if (entry.roles?.length > 0) return isInRoles(entry.roles);

  return true;
};

const parseRoles = (roles) => {
  if(typeof roles == 'string' && (<string>roles).indexOf('[') > -1)
    return JSON.parse((<string>roles).replace(/'/g, "\""));
  
  return roles;
}
