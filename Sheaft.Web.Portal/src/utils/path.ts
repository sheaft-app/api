export const parseActivePath = (path) => {
  if(!path || path.length < 1)
    return '/index';
  
  let indexOf = path.indexOf('/');
  if(indexOf > -1 && path.indexOf('/') == path.length - 1)
    path += "index";
  
  let split = path.split('/:');
  if(split && split.length > 1)
    path = split[0] + "/index";
  
  return path;
}
