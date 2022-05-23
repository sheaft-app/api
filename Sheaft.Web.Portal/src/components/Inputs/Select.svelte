<script lang='ts'>
  import './inputs.scss'

  export let value: string = ''
  export let label: string = ''
  export let options: { label: string, value: string }[] = []
  export let isLoading: boolean = false
</script>

<div class='f-input'>
  {#if label && label.length > 0}
    <label class='block mb-2 text-sm text-gray-900'>{label}</label>
  {/if}
  <select disabled={isLoading} bind:value class={$$props.class}>
    {#each options as option}
      <option value={option.value}>{option.label}</option>
    {/each}
  </select>
</div>
