<script lang='ts'>
  import './inputs.scss'
  import { nanoid } from 'nanoid'

  export let value: boolean = false
  export let label: string = null
  export let isLoading: boolean = false
  export let required: boolean = true

  const id = nanoid(10)
</script>

<input
  id='{id}'
  type='checkbox'
  bind:checked='{value}'
  disabled='{isLoading}'
  required='{required}'
  class='{$$props.class} w-4 h-4 m-1'
/>
{#if label}
  <label for='{id}' class='form-check-label inline-block text-gray-800'>
    {label}
    {required ? "*" : ""}
  </label>
{/if}
