<script lang="ts">
  import { layout } from "@roxi/routify";
  import Menu from "./Menu.svelte";

  const menuDefinition = {};

  const parseAndAssignToMenus = (children, menus, parent?) => {
    children.forEach(c => {
      let selector = c.meta.group ?? parent;
      if (selector && !menus[selector]) {
        menus[selector] = {
          title: c.meta.group,
          icon: c.meta.icon,
          pages: [],
          visible: c.meta.menu ?? false,
          referenced: (c.meta.index > 0 || c.meta.index) ?? false
        };
      }

      //avoid referencing _layout files
      if (c.__file.isFile && !c.__file.isDir) {
        let group = menus[selector];
        if (group) {
          group.pages.push({
            path: c.path,
            title: c.title,
            icon: c.meta.icon,
            pages: [],
            visible: c.meta.menu ?? false,
            referenced: (c.meta.index > 0 || c.meta.index) ?? false,
            parent: group
          });
        } else
          menus[c.path] = {
            path: c.path,
            title: c.title,
            icon: c.meta.icon,
            pages: [],
            visible: c.meta.menu ?? false,
            referenced: (c.meta.index > 0 || c.meta.index) ?? false
          };
      }

      if (c.children && c.children.length > 0)
        parseAndAssignToMenus(c.children, menus, selector);
    });

    return menus;
  };

  const orderMenus = (items): Array<any> => {
    return items;
  };

  const menus = parseAndAssignToMenus($layout.children, menuDefinition);
  const entries = orderMenus(menus);
</script>

<nav>
  <Menu entries="{entries}" />
</nav>

<style lang="scss">
  nav {
    @apply w-40 h-full bg-gray-100;
  }
</style>
