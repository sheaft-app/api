export const formatInnerHtml = (node: HTMLElement, formatFunction: Function): void => {
  node.innerHTML = formatFunction(node.innerHTML);
};
