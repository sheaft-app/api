import { derived, writable } from 'svelte/store'
import type { IEmptyResult, IListResult, IResult } from '$types/http'
import { StatusCode } from '$enums/http'
import { api } from '$configs/axios'
import type { Client, Paths, Components } from '$types/api'

interface IReturnableStore {
  returnables: Components.Schemas.ReturnableDto[] | null | undefined,
  returnable: Components.Schemas.ReturnableDto | null | undefined
}

const store = writable(<IReturnableStore>{
  returnables: [],
  returnable: null
})

export const returnables = derived(
  [store],
  ([$store]) => $store.returnables
)

export const returnable = derived(
  [store],
  ([$store]) => $store.returnable
)

export const listReturnables = async (page: number, take: number): Promise<IListResult<Components.Schemas.ReturnableDtoPaginatedResults>> => {
  try {
    const client = await api.getClient<Client>()
    const result = await client.ListReturnables({ page, take })
    store.update(ps => {
      ps.returnables = result.data.items
      return ps
    })
    return Promise.resolve({ success: true, status: StatusCode.OK, data: result.data })
  } catch (exc) {
    console.error(exc)
    return Promise.resolve({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ''
    })
  }
}

export const getReturnable = async (identifier: string): Promise<IResult<Components.Schemas.ReturnableDto>> => {
  try {
    const client = await api.getClient<Client>()
    const result = await client.GetReturnable({ id: identifier })
    store.update(ps => {
      ps.returnable = result.data
      return ps
    })
    return Promise.resolve({ success: true, status: StatusCode.OK, data: result.data })
  } catch (exc) {
    console.error(exc)
    return Promise.resolve({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ''
    })
  }
}

export const create = async (returnable: Paths.CreateReturnable.RequestBody): Promise<IResult<string>> => {
  try {
    const client = await api.getClient<Client>()
    const result = await client.CreateReturnable(null, returnable)
    return Promise.resolve({
      success: true,
      status: StatusCode.CREATED,
      data: result.data
    })
  } catch (exc) {
    console.error(exc)
    return Promise.reject({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ''
    })
  }
}
export const update = async (id: string, returnable: Paths.UpdateReturnable.RequestBody |undefined): Promise<IEmptyResult> => {
  try {
    const client = await api.getClient<Client>()
    const result = await client.UpdateReturnable(id, returnable)
    return Promise.resolve({
      success: true,
      status: StatusCode.CREATED
    })
  } catch (exc) {
    console.error(exc)
    return Promise.reject({
      success: false,
      status: StatusCode.BAD_REQUEST,
      message: ''
    })
  }
}
