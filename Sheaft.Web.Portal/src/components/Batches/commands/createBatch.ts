import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import { get } from 'svelte/store'
import type { IAuthStore } from '$components/Account/store'
import type { BatchDateKind } from '$components/Batches/enums'

export class CreateBatchCommand extends Request<Promise<string>> {
  constructor(
    public number: string,
    public dateKind: BatchDateKind,
    public expirationDate: string,
    public productionDate?: string
  ) {
    super()
  }
}

export class CreateBatchHandler {
  constructor(private _client: Client, private _authStore: IAuthStore) {
  }

  handle = async (request: CreateBatchCommand): Promise<string> => {
    try {
      const profileId = get(this._authStore).account.profile.id
      const { data } = await this._client.CreateBatch({ supplierId: profileId }, request)
      return Promise.resolve(data)
    } catch
      (exc) {
      console.error(exc)
      return Promise.reject({ error: exc })
    }
  }

}
