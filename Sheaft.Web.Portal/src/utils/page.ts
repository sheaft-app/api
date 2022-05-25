export const checkPageAccess = (
  from: string,
  to: string,
  meta: any,
  isAuthenticated: boolean,
  isRegistered: boolean,
  returnUrl: string,
  isInRole: any
): { path: string; params?: any } | null => {
  if (
    to == "/_fallback" ||
    to == "/unauthorized" ||
    meta.public ||
    (meta.anonymous && !isAuthenticated)
  )
    return null;

  if (meta.anonymous && isAuthenticated)
    return {
      path: from
    };

  if (meta.roles && meta.roles.length > 0 && (!isAuthenticated || !isInRole(meta.roles)))
    return { path: "/unauthorized" };

  if (!meta.anonymous && !meta.public && !isAuthenticated) {
    return {
      path: "/auth/login",
      params: {
        returnUrl: returnUrl ?? `${window.location.pathname}${window.location.search}`
      }
    };
  }

  if (from != "/auth/configure" && to != "/auth/configure" && !isRegistered) {
    return {
      path: "/auth/configure",
      params: {
        returnUrl: returnUrl ?? `${window.location.pathname}${window.location.search}`
      }
    };
  }

  return null;
};
