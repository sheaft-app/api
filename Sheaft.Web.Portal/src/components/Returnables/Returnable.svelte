<script lang='ts'>
  import type { ReturnableForm } from '$components/Returnables/types'
  import Input from '$components/Input/Input.svelte'
  import Checkbox from '$components/Checkbox/Checkbox.svelte'
  import Vat from '$components/Vat/Vat.svelte'
  import { calculateOnSalePrice } from '$utils/money'
  import type { KeyedWritable } from '@felte/common'

  export let data: KeyedWritable<ReturnableForm>
  export let disabled: boolean

  $: onSalePrice = calculateOnSalePrice($data.unitPrice, $data.vat)

  $: if ($data.hasVat && (!$data.vat || $data.vat == 0)) {
    $data.vat = 5.5
  } else if (!$data.hasVat && $data.vat && $data.vat > 0) {
    $data.vat = 0
  }
</script>

<Input
  id='name'
  label='Nom'
  bind:value='{$data.name}'
  placeholder='Le nom de votre consigne'
  disabled='{disabled}' />
<Input
  id='unitPrice'
  label='Prix HT'
  bind:value='{$data.unitPrice}'
  placeholder='Prix HT de votre consigne en €'
  disabled='{disabled}' />
<Checkbox
  id='hasVat'
  label='Je facture la TVA pour cette consigne'
  disabled='{disabled}'
  bind:value='{$data.hasVat}'
  class='my-3' />
{#if $data.hasVat}
  <Vat id='vat' label='TVA' bind:value='{$data.vat}' disabled='{disabled}' />
  <Input
    id='onSalePrice'
    type='number'
    label='Prix TTC (calculé)'
    value='{onSalePrice}'
    disabled='{true}'
    required='{false}' />
{/if}
