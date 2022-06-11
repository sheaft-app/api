<script lang="ts">
  import { authStore } from "$stores/auth";
  import { checkPageAccess } from "$utils/page";
  import { page, goto, beforeUrlChange, params } from "@roxi/routify";
  import SideNav from "$components/Nav/SideNav.svelte";
  import TopNav from "$components/Nav/TopNav.svelte";
  import Screen from "$components/Screen.svelte";

  $beforeUrlChange((event: any, route) => {
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
  });

  $: {
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
