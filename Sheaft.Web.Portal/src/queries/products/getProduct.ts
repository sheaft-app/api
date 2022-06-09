import type { Client } from '$types/api'
import { Request } from 'jimmy-js'
import type { Components } from '$types/api'

export class GetProductQuery extends Request<Promise<Components.Schemas.ProductDto>> {
  constructor(public id:string) {
    super();
  }
}

export class GetProductHandler {
  constructor(private _client: Client) {}

  handle = async (request: GetProductQuery):Promise<Components.Schemas.ProductDto> => {
    try {
      const { data } = await this._client.GetProduct(request)
      return Promise.resolve(data)
    } catch (exc) {
      console.error(exc)
      return Promise.reject({ error : exc })
    }
  }
}
