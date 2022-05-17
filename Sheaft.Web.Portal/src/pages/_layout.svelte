<script lang='ts'>
  import { isAuthenticated, userIsInRoles } from '$stores/auth'
  import { checkPageAccess } from '$utils/page'
  import { page, goto, beforeUrlChange, params } from '@roxi/routify'
  import SideNav from '$components/Nav/SideNav.svelte'
  import TopNav from '$components/Nav/TopNav.svelte'
  import Screen from '$components/Screen.svelte'

  $beforeUrlChange((event, route) => {
    if (route.path.indexOf('/auth') > -1)
      return true

    const pageAccessResult = validateAccessToPage(
      route.path,
      route.meta,
      $isAuthenticated
    )
    if (!pageAccessResult)
      return true

    $goto(pageAccessResult.path, pageAccessResult.params)
    return false
  })

  const validateAccessToPage = (
    path: string,
    meta: any,
    isAuthenticated: boolean
  ): any | null => {
    const url = checkPageAccess(path, meta.redirectIfAuthenticated, isAuthenticated)
    if (url) return { path: url }

    if (
      meta.roles &&
      meta.roles.length > 0 &&
      (!isAuthenticated || !userIsInRoles(meta.roles))
    ) {
      return { path: '/unauthorized' }
    }

    if (!meta.public && !isAuthenticated)
      return {
        path: '/auth/login',
        params: { returnUrl: $params.returnUrl ?? `${window.location.pathname}${window.location.search}` }
      }
  }

  $: {
    const pageAccessResult = validateAccessToPage(
      $page.path,
      $page.meta,
      $isAuthenticated
    )
    if (pageAccessResult)
      $goto(pageAccessResult.path, pageAccessResult.params)
  }
</script>

<main class='flex bg-back-100'
      class:flex-col={!$isAuthenticated}
      class:flex-row={$isAuthenticated}>
  {#if $isAuthenticated}
    <SideNav />
  {:else}
    <TopNav />
  {/if}
  <Screen>
    <slot />
  </Screen>
</main>
