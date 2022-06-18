<script lang="ts">
  import { page, goto, beforeUrlChange, params } from "@roxi/routify";
  import { checkPageAccess } from "./security";
  import { authStore } from "$components/Account/store";
  import SideNav from "$components/Nav/SideNav.svelte";
  import TopNav from "$components/Nav/TopNav.svelte";
  import Screen from "$components/Layout/Screen.svelte";

  $beforeUrlChange((event: any, route) => {
    const accessToParentModuleIsRefused =
      route.parent?.meta?.roles && !authStore.userIsInRoles(route.parent.meta.roles);
    if (accessToParentModuleIsRefused) {
      $goto("/unauthorized");
      return false;
    } else {
      const pageAccessResult = checkPageAccess(
        route?.path,
        event.url,
        route?.meta,
        $authStore.isAuthenticated,
        $authStore.isRegistered,
        $params.returnUrl ?? event.url,
        authStore.userIsInRoles
      );
      if (!pageAccessResult) return true;

      $goto(pageAccessResult.path, pageAccessResult.params);
      return false;
    }
  });

  $: {
    const accessToParentModuleIsRefused =
      $page.parent?.meta?.roles && !authStore.userIsInRoles($page.parent.meta.roles);
    if (accessToParentModuleIsRefused) {
      $goto("/unauthorized");
    } else {
      const pageAccessResult = checkPageAccess(
        "/",
        $page.path,
        $page.meta,
        $authStore.isAuthenticated,
        $authStore.isRegistered,
        $params.returnUrl,
        authStore.userIsInRoles
      );
      if (pageAccessResult) $goto(pageAccessResult.path, pageAccessResult.params);
    }
  }
</script>

<main
  class="flex bg-back-100"
  class:flex-col="{!$authStore.isAuthenticated}"
  class:flex-row="{$authStore.isAuthenticated}"
>
  {#if $authStore.isAuthenticated}
    <SideNav />
  {:else}
    <TopNav />
  {/if}
  <Screen>
    <slot />
  </Screen>
</main>
