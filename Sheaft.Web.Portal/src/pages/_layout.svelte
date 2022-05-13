<script lang="ts">
  import { getAuthStore } from "$stores/auth";
  import { page, goto, url, beforeUrlChange } from "@roxi/routify";
  import Nav from "$components/Nav.svelte";
  import Screen from '$components/Screen.svelte'

  const authStore = getAuthStore();
  const isAuthenticated = authStore.isAuthenticated;

  const getReturnUrl = (path, params) => {
    let returnUrl = `${path ?? "/"}${params && params.length > 0 ? "?" : ""}`;
    Object.entries(params).forEach(([key, value]) => {
      if (value) returnUrl += `&${key}=${value}`;
      else returnUrl += `&${key}`;
    });

    return returnUrl;
  };

  $beforeUrlChange((event, route) => {
    if (route.meta.redirectIfAuthenticated && $isAuthenticated) {
      $goto("/");
      return false;
    }
    
    if (route.meta.roles && route.meta.roles.length > 0 && (!$isAuthenticated || !authStore.isInRoles(route.meta.roles))) {
      $goto("/unauthorized");
      return false;
    }

    if (route.path != "/auth/login" && !route.meta.public && !$isAuthenticated) {
      $goto("/auth/login", {
        returnUrl: getReturnUrl(event.state?.path, event.state?.params)
      });
      return false;
    }
    return true;
  });

  if ($page.meta.redirectIfAuthenticated && $isAuthenticated) {
    $goto("/");
  }

  if ($page.meta.roles && $page.meta.roles.length > 0 && (!$isAuthenticated || !authStore.isInRoles($page.meta.roles))) {
    $goto("/unauthorized");
  }

  if (!$page.meta.public && !$isAuthenticated)
    $goto("/auth/login", {
      returnUrl: `${window.location.pathname}${window.location.search}`
    });
</script>

<main>
  <Nav />
  <Screen>
    <slot />
  </Screen>
</main>

<style lang="scss">
  main {
    display: flex;
  }
</style>
