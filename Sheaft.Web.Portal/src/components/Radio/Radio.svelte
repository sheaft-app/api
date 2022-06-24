<script lang='ts'>
  import './radio.scss'
  import { nanoid } from 'nanoid'
  import type { RadioOption } from '$components/Radio/types'

  export let id: string | undefined
  export let value: number | undefined
  export let label: string | undefined
  export let disabled: boolean = false
  export let required: boolean = true
  export let values: string[] | RadioOption[] = []

  if (!id) id = nanoid(10)
</script>

<fieldset class='my-4'>
  {#if label}
    <label class='block mb-2 text-sm font-medium text-gray-700 '
    >{label} {required ? "*" : ""}</label>
  {/if}
  <div id='{id}' class='flex' aria-describedby='{id}-validation'>
    {#each values as radioValue}
      <div class='f-radio'>
        {#if radioValue.value === null || radioValue.value === undefined}
          <input
            class='peer'
            type='radio'
            bind:group='{value}'
            id='{id}-{radioValue}'
            value='{radioValue}'
            disabled='{disabled}' />
          <label
            class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
            for='{id}-{radioValue}'>{radioValue}</label>
        {:else}
          <input
            class='peer'
            type='radio'
            bind:group='{value}'
            id='{id}-{radioValue.value}'
            value='{radioValue.value}'
            disabled='{disabled}' />
          <label
            class='peer-checked:bg-primary-500 peer-checked:font-bold peer-checked:text-white hover:peer-checked:bg-primary-600'
            for='{id}-{radioValue.value}'>{radioValue.label}</label>
        {/if}
      </div>
    {/each}
  </div>
  <div
    id='{id}-validation'
    class='validation-reporter'
    data-felte-reporter-dom-for='{id}'>
  </div>
  <div
    id='{id}-warning'
    class='warning-reporter'
    data-felte-reporter-dom-for='{id}'
    data-felte-reporter-dom-level='warning'>
  </div>
</fieldset>
