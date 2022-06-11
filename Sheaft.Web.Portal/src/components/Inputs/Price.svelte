<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id: string | null = null;
  export let label: string | null = null;
  export let value: number = 0;
  export let min: number | null = null;
  export let max: number | null = null;
  export let placeholder: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;
  export let disabled: boolean = false;

  let name: string | null = null;
  if (!id) id = nanoid(10);
  else name = id;
</script>

<div class="f-input">
  {#if label}
    <label for="{id}">{label} {required ? "*" : ""}</label>
  {/if}
  <input
    id="{id}"
    name="{name}"
    type="number"
    disabled="{disabled || isLoading}"
    placeholder="{placeholder}"
    bind:value
    class="{$$props.class}"
    required="{required}"
    min="{min ?? 0}"
    max="{max ?? 9999}"
  />
</div>
