import { validator } from "@felte/validator-vest";
import reporterDom from "@felte/reporter-dom";
import type { Suite } from "vest";

export const getFormValidators = (suite: Suite<any>) => {
  return [<any>validator({ suite }), reporterDom({ single: true })];
};
