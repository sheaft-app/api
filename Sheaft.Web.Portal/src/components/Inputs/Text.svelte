<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let value: string = "";
  export let label: string = "";
  export let placeholder: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;
  export let minLength: number = 0;
  export let maxLength: number = 250;
  
  const id = nanoid(10);
</script>

<div class="f-input">
  <label for="{id}">{label} {required ? '*' : ''}</label>
  <input
    id="{id}"
    type="text"
    disabled="{isLoading}"
    placeholder="{placeholder}"
    bind:value
    class="{$$props.class}"
    required='{required}'
    minLength={minLength}
    maxLength={maxLength}
  />
</div>
