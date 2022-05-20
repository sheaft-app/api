export const checkPageAccess = (
  path: string,
  meta: any,
  isAuthenticated: boolean,
  isRegistered : boolean,
  returnUrl:string,
  isInRole: any
): { path:string, params?:any } | null => {  
  if(path == '/auth/configure' && isRegistered)
    return { path : '/' };
  
  if (path == "/_fallback" || path == "/unauthorized" || path.indexOf('/auth') > -1) 
    return null;

  if (meta.redirectIfAuthenticated && isAuthenticated) 
    return { path:meta.redirectIfAuthenticated.length > 0 ?  meta.redirectIfAuthenticated : "/"};

  if (
    meta.roles &&
    meta.roles.length > 0 &&
    (!isAuthenticated || !isInRole(meta.roles))
  )
    return { path: '/unauthorized' }

  if (!meta.public && !isAuthenticated)
    return {
      path: '/auth/login',
      params: { returnUrl: returnUrl ?? `${window.location.pathname}${window.location.search}` }
    }

  if(!isRegistered)
    return {
      path: '/auth/configure',
      params: { returnUrl: returnUrl ?? `${window.location.pathname}${window.location.search}` }
    }

  return null;
};
