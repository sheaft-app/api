<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";
  import type { ISelectOption } from './types'

  export let id: string | null = null;
  export let value: string | null = null;
  export let label: string | null = null;
  export let options: ISelectOption[] = [];
  export let required: boolean = true;
  export let disabled: boolean = false;

  let name: string | null = null;
  if (!id) id = nanoid(10);
  else name = id;
</script>

<div class="f-input">
  {#if label && label.length > 0}
    <label for="{id}">{label} {required ? "*" : ""}</label>
  {/if}
  <select
    id="{id}"
    name="{name}"
    disabled="{disabled}"
    bind:value
    class="{$$props.class}"
  >
    {#each options as option}
      <option value="{option.value}">{option.label}</option>
    {/each}
  </select>
  <div
    id="{id}-validation"
    class='validation-reporter'
    data-felte-reporter-dom-for="{id}"
  ></div>
  <div
    id="{id}-warning"
    class='warning-reporter'
    data-felte-reporter-dom-for="{id}"
    data-felte-reporter-dom-level="warning"
  ></div>
</div>
