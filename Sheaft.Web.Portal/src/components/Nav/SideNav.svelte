<script lang="ts">
  import { layout } from "@roxi/routify";
  import Menu from "./Menu.svelte";
  import { parseLayoutTree } from "./menus";
  import "./nav.scss";
  import Logo from "$components/Nav/Logo.svelte";
  import logoSrc from "$assets/logos/sheaft-colorized.svg?url";
  import UserMenu from '$components/Auth/UserMenu.svelte'

  const menus = parseLayoutTree($layout.children);
</script>

<nav class="w-80 h-full flex flex-col justify-between bg-white">
  <Logo width="{80}" src="{logoSrc}" />
  <Menu entries="{menus}" expand="{true}" />
  <UserMenu class="w-full self-end" />
</nav>
