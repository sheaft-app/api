﻿import { derived, writable } from "svelte/store";
import axios from "axios";
import type { IListResult, IResult } from "$types/http";
import { StatusCode } from "$enums/http";
import type { ICreateProduct } from "$types/product";

const productsStore = writable({
  products: [],
  product: null
});

export const products = derived(
  [productsStore],
  ([$productsStore]) => $productsStore.products
);
export const productsCount = derived(
  [productsStore],
  ([$productsStore]) => $productsStore.products?.length ?? 0
);

export const list = async (): Promise<IListResult<any>> => {
  try {
    const result = await axios.get("/api/products");
    productsStore.update(ps => {
      ps.products = result.data.items;
      return ps;
    });
    return Promise.resolve({ success: true, status: StatusCode.OK, data: result.data });
  } catch (exc) {
    console.error(exc);
    return Promise.reject({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ""
    });
  }
};

export const get = async (identifier: string): Promise<IResult<any>> => {
  try {
    const result = await axios.get(`/api/products/${identifier}`);
    productsStore.update(ps => {
      ps.product = result.data;
      return ps;
    });
    return Promise.resolve({ success: true, status: StatusCode.OK, data: result.data });
  } catch (exc) {
    console.error(exc);
    return Promise.reject({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ""
    });
  }
};

export const create = async (product: ICreateProduct): Promise<IResult<string>> => {
  try {
    const result = await axios.post("/api/products", product);
    return Promise.resolve({
      success: true,
      status: StatusCode.CREATED,
      data: result.data
    });
  } catch (exc) {
    console.error(exc);
    return Promise.reject({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ""
    });
  }
};
