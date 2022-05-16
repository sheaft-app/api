<script lang="ts">
  import { beforeUrlChange, goto, page } from "@roxi/routify";
  import { getAuthStore } from "$stores/auth";
  import { checkPageAccess } from "$utils/page";

  const authStore = getAuthStore();
  const isAuthenticated = authStore.isAuthenticated;

  $beforeUrlChange((event, route) => {
    const url = checkPageAccess(
      route.path,
      route.meta.redirectIfAuthenticated,
      $isAuthenticated
    );
    if (!url) return true;

    $goto(url);
    return false;
  });

  $: {
    const url = checkPageAccess(
      $page.path,
      $page.meta.redirectIfAuthenticated,
      $isAuthenticated
    );
    if (url) $goto(url);
  }
</script>

<slot />
