<script lang="ts">
  import { layout } from "@roxi/routify";
  import Menu from "./Menu.svelte";
  import User from "$components/Nav/User.svelte";
  import { parseLayoutTree } from "./menus";
  import "./nav.scss";
  import Logo from "$components/Nav/Logo.svelte";
  import ffd from "$assets/logos/sheaft-colorized.svg?url";

  const menus = parseLayoutTree($layout.children);
</script>

<nav class="w-80 h-full flex flex-col justify-between bg-white">
  <Logo width="{80}" src="{ffd}" />
  <Menu entries="{menus}" expand="{true}" />
  <User class="w-full self-end" />
</nav>
