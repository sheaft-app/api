export const checkPageAccess = (
  path: string,
  redirectIfAuthenticated: boolean,
  isAuthenticated: boolean
): string | null => {
  if (path == "/_fallback") return null;

  if (redirectIfAuthenticated && isAuthenticated) return "/";

  return null;
};
