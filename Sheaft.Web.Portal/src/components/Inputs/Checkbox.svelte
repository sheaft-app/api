<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id: string | null = null;
  export let value: boolean = false;
  export let label: string | null = null;
  export let disabled: boolean = false;
  export let required: boolean = false;

  let name: string | null = null;
  if (!id) id = nanoid(10);
  else name = id;
</script>

<label for="{id}" class="form-check-label inline-block text-gray-800 cursor-pointer">
  <input
    id="{id}"
    name="{name}"
    type="checkbox"
    bind:checked="{value}"
    disabled="{disabled}"
    required="{required}"
    class="{$$props.class} w-4 h-4 m-1"
  />
  {#if label}
    {label}
    {required ? "*" : ""}
  {/if}
</label>
