<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let value: string = "";
  export let label: string = "";
  export let options: { label: string; value: string }[] = [];
  export let isLoading: boolean = false;
  export let required: boolean = true;

  const id = nanoid(10);
</script>

<div class="f-input">
  {#if label && label.length > 0}
    <label for="{id}">{label} {required ? '*' : ''}</label>
  {/if}
  <select 
    id="{id}" 
    disabled="{isLoading}" 
    bind:value 
    class="{$$props.class}" 
    required='{required}'>
    {#each options as option}
      <option value="{option.value}">{option.label}</option>
    {/each}
  </select>
</div>
