<script lang='ts'>
  import { authStore } from '$stores/auth';
  import { page, goto, url, beforeUrlChange } from '@roxi/routify';

  const isAuthenticated = authStore.isAuthenticated;

  const getReturnUrl = (path, params) => {
    let returnUrl = `${path ?? '/'}${params && params.length > 0 ? '?': ''}`;
    Object.entries(params).forEach( ([key, value]) => {
      if(value)
        returnUrl += `&${key}=${value}`;
      else
        returnUrl += `&${key}`;
    });
    
    return returnUrl;
  }
  
  $beforeUrlChange((event, route) => {
    if (route.path != '/auth/login' && !route.meta.anonymous && !$isAuthenticated) {
      $goto('/auth/login', { returnUrl: getReturnUrl(event.state?.path, event.state?.params)});
      return false;
    }
    return true
  });

  if (!$page.meta.anonymous && !$isAuthenticated)
    $goto('/auth/login', { returnUrl: `${window.location.pathname}${window.location.search}`});

</script>

<slot />
