<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let value: string = "";
  export let label: string = "";
  export let placeholder: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;

  const id = nanoid(10);
</script>

<div class="f-input">
  <label for="{id}">{label} {required ? "*" : ""}</label>
  <input
    id="{id}"
    type="text"
    disabled="{isLoading}"
    placeholder="{placeholder}"
    bind:value
    class="{$$props.class}"
    pattern="[0-9]"
    maxlength="14"
    minlength="14"
    required="{required}"
  />
</div>
