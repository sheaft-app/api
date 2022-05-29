import { derived, writable } from "svelte/store";
import type { IListResult, IResult } from "$types/http";
import { StatusCode } from "$enums/http";
import type { ICreateProduct } from "./types";
import { api } from '$configs/axios'
import type { Client } from '$types/api'

const store = writable({
  products: [],
  product: null
});

export const products = derived(
  [store],
  ([$store]) => $store.products
);

export const product = derived(
  [store],
  ([$store]) => $store.product
);

export const listProducts = async (page:number, take:number): Promise<IListResult<any>> => {
  try {
    const client = await api.getClient<Client>();
    const result = await client.ListProducts({page, take});
    store.update(ps => {
      ps.products = result.data.items;
      return ps;
    });
    return Promise.resolve({ success: true, status: StatusCode.OK, data: result.data });
  } catch (exc) {
    console.error(exc);
    return Promise.resolve({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ""
    });
  }
};

export const getProduct = async (identifier: string): Promise<IResult<any>> => {
  try {
    const client = await api.getClient<Client>();
    const result = await client.GetProduct({id:identifier});
    store.update(ps => {
      ps.product = result.data;
      return ps;
    });
    return Promise.resolve({ success: true, status: StatusCode.OK, data: result.data });
  } catch (exc) {
    console.error(exc);
    return Promise.resolve({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ""
    });
  }
};

export const create = async (product: ICreateProduct): Promise<IResult<string>> => {
  try {
    const client = await api.getClient<Client>();
    const result = await client.CreateProduct(null, product);
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
