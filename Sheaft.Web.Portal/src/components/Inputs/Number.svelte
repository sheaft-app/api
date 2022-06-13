<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id: string | null = null;
  export let label: string | null = null;
  export let value: number = 0;
  export let min: number | null = null;
  export let max: number | null = null;
  export let placeholder: string = "";
  export let disabled: boolean = false;
  export let required: boolean = true;

  let name: string | null = null;
  if (!id) id = nanoid(10);
  else name = id;
</script>

<div class="f-input {$$props.class}">
  {#if label}
    <label for="{id}">{label} {required ? "*" : ""}</label>
  {/if}
  <input
    id="{id}"
    name="{name}"
    type="number"
    disabled="{disabled}"
    placeholder="{placeholder}"
    bind:value
    required="{required}"
    min="{min ?? 0}"
    max="{max ?? 99999}"
  />
</div>
