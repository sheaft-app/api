<script lang="ts">
  import { layout, page } from "@roxi/routify";
  import "./nav.scss";
  import { authStore } from "$components/Auth/auth";
  import Logo from "$components/Nav/Logo.svelte";
  import UserMenu from '$components/Auth/UserMenu.svelte'
</script>

<nav class="w-full flex flex-row items-center bg-primary-700">
  <Logo width="{40}" />
  <div class="flex-grow">
    <!--    <Menu entries="{menus}" expand="{true}" />-->
  </div>
  {#if $authStore.isAuthenticated || $page.path.indexOf("/account") < 0}
    <UserMenu class="mr-4" />
  {/if}
</nav>
