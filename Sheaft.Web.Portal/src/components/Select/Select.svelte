<script lang="ts">
  import "./select.scss";
  import { nanoid } from "nanoid";
  import type { SelectOption } from "./types";

  export let id: string | undefined;
  export let name: string | undefined;
  export let value: string | undefined;
  export let label: string | undefined;
  export let options: SelectOption[] = [];
  export let required: boolean = true;
  export let disabled: boolean = false;

  if (!id) id = nanoid(10);
  else if (!name) name = id;
</script>

<div class='flex justify-between'>
  {#if label}
    <label class='flex-grow' for='{id}'>{label} {required ? "*" : ""}</label>
  {/if}
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
</div>
<select
  id="{id}"
  name="{name}"
  disabled="{disabled}"
  bind:value
  class="{$$props.class}"
  aria-describedby="{id}-validation">
  {#each options as option}
    <option value="{option.value}">{option.label}</option>
  {/each}
</select>
