<script lang="ts">
  import "./inputs.scss";
  import { nanoid } from "nanoid";
  import {setType} from './setType'

  export let id: string | null = null;
  export let type: string = "text";
  export let value: string | null = null;
  export let label: string | null = null;
  export let placeholder: string = "";
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
    name="{id}"
    use:setType={type}
    disabled="{disabled}"
    placeholder="{placeholder}"
    bind:value
    class="{$$props.class}"
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
