<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id:string|null=null;
  export let value: string|null = null;
  export let label: string|null = null;
  export let options: { label: string; value: string }[] = [];
  export let isLoading: boolean = false;
  export let required: boolean = true;
  export let disabled: boolean = true;

  let name:string|null = null;
  if(!id)
    id = nanoid(10);
  else
    name = id;
</script>

<div class="f-input">
  {#if label && label.length > 0}
    <label for="{id}">{label} {required ? "*" : ""}</label>
  {/if}
  <select
    id='{id}'
    name='{name}'
    disabled="{isLoading||disabled}"
    bind:value
    class="{$$props.class}"
    required="{required}"
  >
    {#each options as option}
      <option value="{option.value}">{option.label}</option>
    {/each}
  </select>
</div>
