<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id:string|null=null;
  export let value: string|null = null;
  export let label: string|null = null;
  export let placeholder: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;
  export let minLength: number = 0;
  export let maxLength: number = 250;

  let name:string|null = null;
  if(!id)
    id = nanoid(10);
  else
    name = id;
</script>

<div class="f-input">
  {#if label}
    <label for='{id}'>{label} {required ? "*" : ""}</label>
  {/if}
  <input
    id="{id}"
    name="{id}"
    type="text"
    disabled="{isLoading}"
    placeholder="{placeholder}"
    bind:value
    class="{$$props.class}"
    required="{required}"
    minLength="{minLength}"
    maxLength="{maxLength}"
  />
</div>
