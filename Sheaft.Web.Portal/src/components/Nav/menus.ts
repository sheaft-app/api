import type { IEntry } from '$components/Nav/types'

const parseAndAssignToMenus = (children: any, menus: any, parent?: string) => {
  children.forEach((c: any) => {
    let selector = c.meta.group ? c.meta.menu : parent;

    //insert or update group
    if (selector) {
      if (c.__file.isDir && c.__file.isFile) {
        if (!menus[selector]) menus[selector] = getMenuEntry(c, c.meta.group);

        menus[selector].public = true;
        menus[selector].icon = c.meta.icon;
        menus[selector].name = c.meta.menu;
        menus[selector].visible = c.meta.menu && c.meta.menu.length > 0;
        menus[selector].referenced = (c.meta.index > 0 || c.meta.index) ?? false;
      } else {
        menus[selector].pages.push(getMenuEntry(c, null, menus[selector]));
      }
    } else {
      menus[c.path] = getMenuEntry(c);
    }

    if (c.children && c.children.length > 0)
      parseAndAssignToMenus(c.children, menus, selector);
  });

  return menus;
};

const getMenuEntry = (route: any, title?: string | null, parent?: any) => {
  return {
    path: route.path,
    name: title ?? route.meta.menu,
    icon: route.meta.icon,
    public: route.meta.public,
    anonymous: route.meta.anonymous,
    pages: [],
    visible: route.meta.menu && route.meta.menu.length > 0,
    referenced: (route.meta.index > 0 || route.meta.index) ?? false,
    default: route.meta.default ?? false,
    roles: route.meta.roles ?? [],
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
  isInRoles: any
): boolean => {
  if (!entry.visible) return false;

  if (entry.public) return true;

  if (!isAuthenticated) return false;

  if (entry.roles && entry.roles.length > 0) return isInRoles(entry.roles);

  return true;
};
