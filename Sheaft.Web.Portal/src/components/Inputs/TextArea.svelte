<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";

  export let id: string | null = null;
  export let value: string | null = null;
  export let label: string | null = null;
  export let placeholder: string = "";
  export let disabled: boolean = false;
  export let required: boolean = false;

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
    bind:value={value}
    disabled="{disabled}"
    placeholder="{placeholder}"
    class="{$$props.class}"
    required="{required}"
    aria-describedby="{id}-validation"/>
  <div
    id="{id}-validation"
    class='validation-reporter'
    data-felte-reporter-dom-for="{id}"
  ></div>
  <div
    id="{id}-warning"
    class='warning-reporter'
    data-felte-reporter-dom-for="{id}"
    data-felte-reporter-dom-level="warning"
  ></div>
</div>
