<script lang='ts'>
  import type { BatchForm } from '$components/Batches/types'
  import Input from '$components/Input/Input.svelte'
  import type { KeyedWritable } from '@felte/common'
  import Radio from '$components/Radio/Radio.svelte'
  import { BatchDateKind } from '$components/Batches/enums'
  import { dateKind } from '$components/Batches/utils'

  export let data: KeyedWritable<BatchForm>
  export let disabled: boolean

</script>

<Input
  id='number'
  label='Numéro de lot'
  bind:value='{$data.number}'
  placeholder='ex: LT20220501-102'
  disabled='{disabled}' />
<Radio
  id='dateKind'
  label='Type de date'
  bind:value='{$data.kind}'
  values='{[{label: "DLC", value: BatchDateKind.DLC},{label: "DDM", value: BatchDateKind.DDM},{label: "DDC", value: BatchDateKind.DDC},{label: "DCR", value: BatchDateKind.DCR}]}'
  disabled='{disabled}' />
{#if $data.kind !== BatchDateKind.DDC}
  <Input
    id='productionDate'
    type='date'
    label='Jour de production'
    bind:value='{$data.productionDate}'
    disabled='{disabled}' />
{/if}
<Input
  id='expirationDate'
  type='date'
  label='{dateKind($data.kind)}'
  bind:value='{$data.expirationDate}'
  disabled='{disabled}' />
