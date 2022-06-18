<script lang="ts">
  import { layout, page } from "@roxi/routify";
  import Logo from '$components/Logo/Logo.svelte'
  import UserMenu from '$components/Account/UserMenu.svelte'
</script>

<nav class="w-full flex flex-row items-center bg-primary-700">
  <Logo width="{40}" />
  <div class="flex-grow">
    <!--    <Menu entries="{menus}" expand="{true}" />-->
  </div>
  <UserMenu class="mr-4" />
</nav>
