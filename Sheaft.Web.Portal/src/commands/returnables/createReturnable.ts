import type { Client } from '$types/api'
import { Request } from 'jimmy-js'

export class CreateReturnableRequest extends Request<Promise<string>> {
  constructor(
    public name: string | null | undefined, 
    public unitPrice:number | undefined, 
    public vat:number | undefined, 
    public code?: string | null | undefined) {
    super();
  }
}

export class CreateReturnableHandler {
  constructor(private _client: Client) {}

  handle = async (request: CreateReturnableRequest):Promise<string> => {
    try {
      const { data } = await this._client.CreateReturnable(null, request)
      return Promise.resolve(data)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error : exc })
    }
  }
}
