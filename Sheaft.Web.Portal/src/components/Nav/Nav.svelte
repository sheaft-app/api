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

      //update group with _layout info
      if (selector) {
        if (c.__file.isDir && c.__file.isFile) {
          if (!menus[selector])
            menus[selector] = {
              title: c.meta.group,
              icon: c.meta.icon,
              pages: [],
              visible: c.meta.menu ?? false,
              referenced: (c.meta.index > 0 || c.meta.index) ?? false,
              default: false
            }

          menus[selector].icon = c.meta.icon
          menus[selector].title = c.meta.group
          menus[selector].visible = c.meta.menu
          menus[selector].referenced = (c.meta.index > 0 || c.meta.index) ?? false
        } else {
          menus[selector].pages.push({
            path: c.path,
            title: c.title,
            icon: c.meta.icon,
            pages: [],
            visible: c.meta.menu ?? false,
            referenced: (c.meta.index > 0 || c.meta.index) ?? false,
            default: c.meta.default ?? false,
            parent: menus[selector]
          })
        }
      } else {
        menus[c.path] = {
          path: c.path,
          title: c.title,
          icon: c.meta.icon,
          pages: [],
          visible: c.meta.menu ?? false,
          referenced: (c.meta.index > 0 || c.meta.index) ?? false,
          default: c.meta.default ?? false
        }
      }

      if (c.children && c.children.length > 0)
        parseAndAssignToMenus(c.children, menus, selector)
    })

    return menus
  }

  const orderMenus = (items): Array<any> => {
    return items
  }

  const menus = parseAndAssignToMenus($layout.children, menuDefinition)
  const entries = orderMenus(menus)
</script>

<nav class='w-80 h-full flex flex-col'>
  <Logo />
  <Menu entries='{entries}' expand='{true}' />
  <User />
</nav>
