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
  <textarea
    id="{id}"
    bind:value
    disabled="{isLoading}"
    placeholder="{placeholder}"
    class="{$$props.class}"
    required="{required}"></textarea>
</div>
