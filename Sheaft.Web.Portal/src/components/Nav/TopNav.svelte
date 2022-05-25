<script lang="ts">
  import { layout, page } from "@roxi/routify";
  import User from "$components/Nav/User.svelte";
  import "./nav.scss";
  import { parseLayoutTree } from "./menus";
  import { isAuthenticated } from "$stores/auth";
  import Logo from "$components/Nav/Logo.svelte";
  import Menu from "$components/Nav/Menu.svelte";

  const menus = parseLayoutTree($layout.children);
</script>

<nav class="w-full flex flex-row items-center bg-primary-700">
  <Logo width="40" />
  <div class="flex-grow">
    <!--    <Menu entries="{menus}" expand="{true}" />-->
  </div>
  {#if $isAuthenticated || $page.path.indexOf("/auth") < 0}
    <User class="mr-4" />
  {/if}
</nav>
