import { derived, get, writable } from 'svelte/store'
import type { Writable } from 'svelte/store'
import type { IDataResult, IResult } from '$types/http'
import { api } from '$configs/axios'
import type { Client, Paths, Components } from '$types/api'
import { goto } from '@roxi/routify'

interface IReturnablesStore {
  returnables: Components.Schemas.ReturnableDto[];
  isLoading: boolean;
}

interface IReturnableStore {
  returnable: Components.Schemas.ReturnableDto;
  isLoading: boolean;
}

const _returnables = writable(<IReturnablesStore>{ returnables: [], isLoading: false })
const _returnable = writable(<IReturnableStore>{ returnable: {}, isLoading: false })

export const returnables = derived(
  [_returnables],
  ([$store]) => $store.returnables
)

export const returnable = derived(
  [_returnable],
  ([$store]) => $store.returnable
)

export const isLoading = derived(
  [_returnables, _returnable],
  ([$returnables, $returnable]) => $returnables.isLoading || $returnable.isLoading
)

export const listReturnables = async (page: number, take: number): Promise<IResult> => {
  try {
    setStoreIsLoading(_returnables, true);
    const client = await api.getClient<Client>()
    const { data } = await client.ListReturnables({ page, take })
    _returnables.update(r => {
      r.returnables = data.items ?? []
      return r
    })
    setStoreIsLoading(_returnables, false);
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

    _returnable.update(r => {
      r.returnable = data;
      return r
    })
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

export const update = async (): Promise<IResult> => {
  try {
    setStoreIsLoading(_returnable, true);
    const client = await api.getClient<Client>()
    const body = get(returnable);
    await client.UpdateReturnable(body.id, body)
    setStoreIsLoading(_returnable, false);
    return Promise.resolve({ success: true })
  } catch (exc) {
    console.error(exc)
    return Promise.resolve({ success: false })
  }
}

export const goToCreate = () => {
  $goto('/returnables/create');
}

export const goToDetails = (id:string) => {
  $goto(`/returnables/${id}`);
}

export const goToList = () => {
  $goto('/returnables/');
}

const setStoreIsLoading = (store: Writable<IReturnablesStore> | Writable<IReturnableStore>, isLoading: boolean): void => {
  store.update((r: any) => {
    r.isLoading = isLoading;
    return r;
  })
}
