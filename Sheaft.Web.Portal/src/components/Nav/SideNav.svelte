<script lang="ts">
  import { layout } from "@roxi/routify";
  import logoSrc from "$assets/logos/sheaft-colorized.svg?url";
  import Logo from '$components/Logo/Logo.svelte'
  import Menu from '$components/Menu/Menu.svelte'
  import UserMenu from '$components/Account/UserMenu.svelte'
  import { parseLayoutTree } from '$components/Menu/menus'

  const menus = parseLayoutTree($layout.children);
</script>

<nav class="w-80 h-full flex flex-col justify-between bg-white">
  <Logo width="{80}" src="{logoSrc}" />
  <Menu entries="{menus}" expand="{true}" />
  <UserMenu class="w-full self-end" />
</nav>
