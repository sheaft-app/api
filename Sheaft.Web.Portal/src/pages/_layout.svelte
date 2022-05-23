<script lang='ts'>
  import { isAuthenticated, isRegistered, userIsInRoles } from '$stores/auth'
  import { checkPageAccess } from '$utils/page'
  import { page, goto, beforeUrlChange, params } from '@roxi/routify'
  import SideNav from '$components/Nav/SideNav.svelte'
  import TopNav from '$components/Nav/TopNav.svelte'
  import Screen from '$components/Screen.svelte'

  $beforeUrlChange((event, route) => {
    const pageAccessResult = checkPageAccess(route.path, event.url, route.meta, $isAuthenticated, $isRegistered, $params.returnUrl ?? event.url, userIsInRoles)
    if (!pageAccessResult)
      return true

    $goto(pageAccessResult.path, pageAccessResult.params)
    return false
  })

  $: {
    const pageAccessResult = checkPageAccess("/", $page.path, $page.meta, $isAuthenticated, $isRegistered, $params.returnUrl, userIsInRoles)
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
