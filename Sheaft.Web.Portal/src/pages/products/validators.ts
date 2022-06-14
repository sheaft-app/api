import { create, enforce, test } from 'vest'
import type { Components } from '$features/api'

export const createProductValidators = create('create-product', (data: Components.Schemas.CreateProductRequest) => {
  test('name', 'Le nom est requis', () => {
    enforce(data.name).isNotEmpty()
  })

  test('unitPrice', 'Le prix est requis', () => {
    enforce(data.unitPrice).isNotEmpty()
  })

  test('unitPrice', 'Le prix doit être supérieur à 0€', () => {
    enforce(data.unitPrice).greaterThan(0)
  })

  test('returnableId', 'Vous devez selectionner une consigne', () => {
    if((<any>data).hasReturnable)
      enforce(data.returnableId).isNotNull()
  })
})
