<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id: string | null = null;
  export let value: string | null = null;
  export let label: string | null = null;
  export let placeholder: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;

  let name: string | null = null;
  if (!id) id = nanoid(10);
  else name = id;
</script>

<div class="f-input">
  {#if label}
    <label for="{id}">{label} {required ? "*" : ""}</label>
  {/if}
  <textarea
    id="{id}"
    name="{name}"
    bind:value
    disabled="{isLoading}"
    placeholder="{placeholder}"
    class="{$$props.class}"
    required="{required}"></textarea>
</div>
