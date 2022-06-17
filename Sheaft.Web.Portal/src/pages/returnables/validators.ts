import { create, enforce, test } from 'vest'
import type { Components } from '$features/api'

export const suite = create('returnable-suite', (data: Components.Schemas.UpdateReturnableRequest) => {
  test('name', 'Le nom est requis', () => {
    enforce(data.name).isNotEmpty()
  })

  test('unitPrice', 'Le prix est requis', () => {
    enforce(data.unitPrice).isNotEmpty()
  })

  test('unitPrice', 'Le prix doit être supérieur à 0€', () => {
    enforce(data.unitPrice).greaterThan(0)
  })

  test('vat', 'Vous devez selectionner un taux de TVA', () => {
    if((<any>data).hasVat)
      enforce(data.vat).isNotNull()
  })

  test('vat', 'Le taux de TVA doit être 5.5%, 10% ou 20%', () => {
    if((<any>data).hasVat)
      enforce(data.vat).inside([5.5,10,20]);
    else
      enforce(data.vat).equals(0);
  })
})
