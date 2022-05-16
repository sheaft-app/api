<script lang='ts'>
  import { layout } from '@roxi/routify'
  import Menu from './Menu.svelte'
  import Logo from '$components/Nav/Logo.svelte'
  import User from '$components/Nav/User.svelte'
  import './nav.scss'

  const menuDefinition = {}

  const parseAndAssignToMenus = (children, menus, parent?) => {
    children.forEach(c => {
      let selector = c.meta.group ?? parent

      //insert or update group
      if (selector) {
        if (c.__file.isDir && c.__file.isFile) {
          if (!menus[selector])
            menus[selector] = getMenuEntry(c, c.meta.group);

          menus[selector].icon = c.meta.icon
          menus[selector].title = c.meta.group
          menus[selector].visible = c.meta.menu
          menus[selector].referenced = (c.meta.index > 0 || c.meta.index) ?? false
        } else {
          menus[selector].pages.push(getMenuEntry(c, null, menus[selector]))
        }
      } else {
        menus[c.path] = getMenuEntry(c);
      }

      if (c.children && c.children.length > 0)
        parseAndAssignToMenus(c.children, menus, selector)
    })

    return menus
  }
  
  const getMenuEntry = (route, title?, parent?) => {
    return {
      path: route.path,
      title: title ?? route.title,
      icon: route.meta.icon,
      pages: [],
      visible: route.meta.menu ?? false,
      referenced: (route.meta.index > 0 || route.meta.index) ?? false,
      default: route.meta.default ?? false,
      parent: parent
    }
  }
  
  const menus = parseAndAssignToMenus($layout.children, menuDefinition)
</script>

<nav class='w-80 h-full flex flex-col'>
  <Logo />
  <Menu entries='{menus}' expand='{true}' />
  <User />
</nav>
