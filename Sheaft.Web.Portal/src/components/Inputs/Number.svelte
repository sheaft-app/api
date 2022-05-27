<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let value: string = "";
  export let label: string = "";
  export let min: number = null;
  export let max: number = null;
  export let placeholder: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;
  export let disabled: boolean = false;
  
  const id = nanoid(10);
</script>

<div class="f-input">
  <label for="{id}">{label} {required ? '*' : ''}</label>
  <input
    id="{id}"
    type="number"
    disabled="{disabled || isLoading}"
    placeholder="{placeholder}"
    bind:value
    class="{$$props.class}"
    required='{required}'
    min='{min ?? 0}'
    max='{max ?? 99999}'
  />
</div>
