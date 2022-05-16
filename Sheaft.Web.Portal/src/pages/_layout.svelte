<script lang="ts">
  import { getAuthStore } from "$stores/auth";
  import { checkPageAccess } from "$utils/page";
  import { page, goto, beforeUrlChange } from "@roxi/routify";
  import Nav from "$components/Nav/Nav.svelte";
  import Screen from "$components/Screen.svelte";

  const authStore = getAuthStore();
  const isAuthenticated = authStore.isAuthenticated;

  const getReturnUrl = (path: string, params) => {
    let returnUrl = `${path ?? "/"}${params && params.length > 0 ? "?" : ""}`;
    Object.entries(params).forEach(([key, value]) => {
      if (value) returnUrl += `&${key}=${value}`;
      else returnUrl += `&${key}`;
    });

    return returnUrl;
  };

  $beforeUrlChange((event, route) => {
    const pageAccessResult = validateAccessToPage(
      route.path,
      route.meta,
      $isAuthenticated
    );
    if (!pageAccessResult) return true;

    $goto(pageAccessResult.path, pageAccessResult.params);
    return false;
  });

  const validateAccessToPage = (
    path: string,
    meta: any,
    isAuthenticated: boolean
  ): any | null => {
    const url = checkPageAccess(path, meta.redirectIfAuthenticated, isAuthenticated);
    if (url) return { path: url };

    if (
      meta.roles &&
      meta.roles.length > 0 &&
      (!isAuthenticated || !authStore.isInRoles(meta.roles))
    ) {
      return { path: "/unauthorized" };
    }

    if (!meta.public && !isAuthenticated)
      return {
        path: "/auth/login",
        params: { returnUrl: `${window.location.pathname}${window.location.search}` }
      };
  };

  $: {
    const pageAccessResult = validateAccessToPage(
      $page.path,
      $page.meta,
      $isAuthenticated
    );
    if (pageAccessResult) $goto(pageAccessResult.path, pageAccessResult.params);
  }
</script>

<main class="flex">
  <Nav />
  <Screen>
    <slot />
  </Screen>
</main>
