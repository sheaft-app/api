<script lang="ts">
  import { beforeUrlChange, goto, page } from "@roxi/routify";
  import { getAuthStore } from "$stores/auth";

  const authStore = getAuthStore();
  const isAuthenticated = authStore.isAuthenticated;

  $beforeUrlChange((event, route) => {
    if (route.path == "/_fallback") return true;

    if (route.meta.redirectIfAuthenticated && $isAuthenticated) {
      $goto("/");
      return false;
    }

    return true;
  });

  const checkPageAccess = (isAuthenticated: boolean) => {
    if ($page.path == "/_fallback") return;

    if ($page.meta.redirectIfAuthenticated && isAuthenticated) {
      $goto("/");
    }
  };

  $: checkPageAccess($isAuthenticated);
</script>

<slot />
