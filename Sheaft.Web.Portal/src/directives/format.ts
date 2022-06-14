export const formatInnerHtml = (node, formatFunction) => {
  node.innerHTML = formatFunction(node.innerHTML);
  return {};
};
