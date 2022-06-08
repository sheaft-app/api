<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id:string|null=null;
  export let value: string|null = null;
  export let label: string|null = null;
  export let placeholder: string = "";
  export let isLoading: boolean = false;
  export let required: boolean = true;

  let name:string|null = null;
  if(!id)
    id = nanoid(10);
  else
    name = id;
</script>

<div class="f-input">
  <label for="{id}">{label} {required ? "*" : ""}</label>
  <input
    id='{id}'
    name='{name}'
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
