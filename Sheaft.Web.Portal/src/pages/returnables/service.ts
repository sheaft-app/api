import { derived, get, writable } from 'svelte/store'
import type { Writable } from 'svelte/store'
import type { IDataResult, IResult } from '$types/http'
import { api } from '$configs/axios'
import type { Client, Paths, Components } from '$types/api'
import { goto } from '@roxi/routify'

const _returnables = writable(<Components.Schemas.ReturnableDto[]>[])

export const returnables = derived(
  [_returnables],
  ([$store]) => $store
)

export const returnable = writable(<Components.Schemas.ReturnableDto>{})

export const listReturnables = async (page: number, take: number): Promise<IResult> => {
  try {
    const client = await api.getClient<Client>()
    const { data } = await client.ListReturnables({ page, take })
    _returnables.set(data.items ?? []);
    return Promise.resolve({ success: true })
  } catch (exc) {
    console.error(exc)
    return Promise.resolve({ success: false })
  }
}

export const getReturnable = async (identifier: string): Promise<IResult> => {
  try {
    const client = await api.getClient<Client>()
    const { data } = await client.GetReturnable({ id: identifier })
    returnable.set(data);
    return Promise.resolve({ success: true })
  } catch (exc) {
    console.error(exc)
    return Promise.resolve({ success: false })
  }
}

export const create = async (request: Paths.CreateReturnable.RequestBody): Promise<IDataResult<string> | IResult> => {
  try {
    const client = await api.getClient<Client>()
    const { data } = await client.CreateReturnable(null, request)
    return Promise.resolve({ success: true, data })
  } catch (exc) {
    console.error(exc)
    return Promise.resolve({ success: false })
  }
}

export const update = async (id:string, request: Paths.UpdateReturnable.RequestBody): Promise<IResult> => {
  try {
    const client = await api.getClient<Client>()
    await client.UpdateReturnable(id, request)
    return Promise.resolve({ success: true })
  } catch (exc) {
    console.error(exc)
    return Promise.resolve({ success: false })
  }
}
