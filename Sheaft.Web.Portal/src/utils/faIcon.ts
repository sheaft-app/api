import * as FasIcons from "@fortawesome/free-solid-svg-icons";
import * as FarIcons from "@fortawesome/free-regular-svg-icons";
import * as FabIcons from "@fortawesome/free-brands-svg-icons";

export const getFaIconFromFullName = (fullName: string) => {
  if(fullName?.length < 1)
    return;
  
  const split = fullName.split("#");
  return getFaIcon(split[0], split[1]);
};

export const getFaIcon = (type: string, name: string) => {
  if(name?.length < 1)
    return null;
  
  let iconName =
    name.indexOf("fa") < 0
      ? `fa${name.substring(0, 1).toUpperCase()}${name.substring(1)}`
      : name;
  switch (type) {
    case "fas":
      return FasIcons[iconName];
    case "far":
      return FarIcons[iconName];
    case "fab":
      return FabIcons[iconName];
    default:
      return null;
  }
};
