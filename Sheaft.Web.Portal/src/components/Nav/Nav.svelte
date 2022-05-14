<script lang="ts">
  import { layout } from "@roxi/routify";
  import Menu from "./Menu.svelte";
  import Logo from '$components/Nav/Logo.svelte'
  import User from '$components/Nav/User.svelte'
  import "./nav.scss";

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
      
      if(selector && menus[selector] && c.__file.isDir){
        let group = menus[selector];
        group.icon = c.meta.icon;
        group.visible = c.meta.menu;
        group.referenced = (c.meta.index > 0 || c.meta.index) ?? false
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

<nav class='w-80 h-full flex flex-col'>
  <Logo />
  <Menu entries="{entries}" />
  <User/>
</nav>
